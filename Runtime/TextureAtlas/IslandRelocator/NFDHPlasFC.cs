using System;
using System.Collections.Generic;
using System.Linq;
using net.rs64.TexTransCore.Island;
using net.rs64.TexTransCore.TransTextureCore.Utils;
using UnityEngine;

namespace net.rs64.TexTransTool.TextureAtlas.IslandRelocator
{
    internal class NFDHPlasFC : IAtlasIslandRelocator
    {
        public bool RectTangleMove => true;

        public bool UseUpScaling { set => _useUpScaling = value; }
        public float Padding { set => _padding = value; }

        public bool _useUpScaling;
        public float _padding;

        public Dictionary<AtlasIslandID, IslandRect> Relocation(Dictionary<AtlasIslandID, IslandRect> atlasIslands, IReadOnlyDictionary<AtlasIslandID, AtlasIsland> atlasIslandReference)
        {
            IslandPoolNextFitDecreasingHeightPlusFloorCeiling(atlasIslands, _useUpScaling, _padding);
            return atlasIslands;
        }
        public static Dictionary<ID, IslandRect> IslandPoolNextFitDecreasingHeightPlusFloorCeiling<ID>(Dictionary<ID, IslandRect> islands, bool useUpScaling = true, float islandPadding = 0.01f, float scaleStep = 0.99f, int safetyCount = 512)
        {
            if (!islands.Any()) return islands;

            var idList = islands.Keys.ToList();
            foreach (var id in idList) { if (islands[id].Size.y > islands[id].Size.x) { var island = islands[id]; island.Rotate90(); islands[id] = island; } }
            idList.Sort((lId, rId) => Mathf.RoundToInt((islands[rId].Size.y - islands[lId].Size.y) * 1073741824));

            ValidateDeceasing(islands, idList);

            if (TryNFDHPlasFC(idList, islands, islandPadding) && !useUpScaling) { return islands; }

            var scale = Mathf.Sqrt(1 / CalculateAllAreaSum(islands.Values));
            ScaleApply(scale);

            var stepCount = 0;
            while (!TryNFDHPlasFC(idList, islands, islandPadding) && safetyCount > stepCount) { ScaleApply(scaleStep); stepCount += 1; }
            if (stepCount == safetyCount) { TTTRuntimeLog.Warning("NextFitDecreasingHeightPlusFloorCeiling : Safetyによりループが中断された可能性があり、アイランドの再配置が正常に行われていない可能性があります"); }

            return islands;

            void ScaleApply(float scale) { foreach (var id in idList) { var island = islands[id]; island.Size *= scale; islands[id] = island; } }
        }

        private static float CalculateAllAreaSum(IEnumerable<IslandRect> islandRects)
        {
            var sum = 0f;
            foreach (var rect in islandRects) { sum += rect.Size.x * rect.Size.y; }
            return sum;
        }

        private static bool ValidateDeceasing<ID>(Dictionary<ID, IslandRect> islands, List<ID> idList)
        {
            var validateHeight = islands[idList[0]].Size.y;
            foreach (var id in idList)
            {
                if (validateHeight >= islands[id].Size.y) { validateHeight = islands[id].Size.y; }
                else
                {
                    TTTRuntimeLog.Warning("NFDHPlusFC : The islands are not sorted correctly according to height. It is possible that undesirable reordering is being done.");
                    return true;
                }
            }
            return false;
        }

        static bool TryNFDHPlasFC<ID>(List<ID> descendingIdList, Dictionary<ID, IslandRect> islandRect, float islandPadding = 0.01f)
        {
            var uvWidthBox = new List<UVWidthBox<IslandRect>>();

            foreach (var islandId in descendingIdList)
            {
                if (TrySetUVBoxList(islandId)) { continue; }

                var Floor = uvWidthBox.Any() ? uvWidthBox.Last().Ceil + islandPadding : islandPadding;
                var Ceil = islandRect[islandId].Size.y + Floor;
                var newWithBox = new UVWidthBox<IslandRect>(Ceil, Floor, islandPadding);

                var island = islandRect[islandId];

                if (!newWithBox.TrySetBox(island, out var pivot)) { return false; }

                island.Pivot = pivot;
                islandRect[islandId] = island;

                uvWidthBox.Add(newWithBox);
            }

            var lastHeight = uvWidthBox.Last().Ceil + islandPadding;
            return lastHeight <= 1;

            bool TrySetUVBoxList(ID islandId)
            {
                var island = islandRect[islandId];
                foreach (var withBox in uvWidthBox)
                {
                    if (withBox.TrySetBox(island, out var pivot))
                    {
                        island.Pivot = pivot;
                        islandRect[islandId] = island;
                        return true;
                    }
                }
                return false;
            }

        }


        private readonly struct UVWidthBox<TIslandRect> where TIslandRect : IIslandRect
        {
            public readonly float Width;
            public readonly float Padding;
            public readonly float Ceil;
            public readonly float Floor;
            public float Height => Ceil - Floor;
            public readonly List<TIslandRect> Upper;
            public readonly List<TIslandRect> Lower;

            public UVWidthBox(float height, float floor, float padding, float width = 1)
            {
                Width = width;

                Ceil = height;
                Floor = floor;
                Padding = padding;

                Upper = new();
                Lower = new();
            }

            public bool TrySetBox(TIslandRect islandRect, out Vector2 outPivot)
            {
                outPivot = Vector2.zero;
                if (Height + 0.01f < islandRect.Size.y) return false;


                var widthMin = Lower.Any() ? Lower.Last().GetMaxPos().x : 0;
                var widthMax = GetCeilWithEmpty(Mathf.Clamp(Floor + islandRect.Size.y + Padding, Floor, Ceil));
                var widthSize = widthMax - widthMin;
                if (widthSize > Padding + islandRect.Size.x + Padding)
                {
                    islandRect.Pivot = outPivot = new Vector2(widthMin + Padding, Floor);
                    Lower.Add(islandRect);
                    return true;
                }


                widthMin = GetFloorWithEmpty(Mathf.Clamp(Ceil - islandRect.Size.y - Padding, Floor, Ceil));
                widthMax = Upper.Any() ? Upper.Last().Pivot.x : Width;
                widthSize = widthMax - widthMin;
                if (widthSize > Padding + islandRect.Size.x + Padding)
                {
                    islandRect.Pivot = outPivot = new Vector2(widthMax - islandRect.Size.x - Padding, Ceil - islandRect.Size.y);
                    Upper.Add(islandRect);
                    return true;
                }


                return false;
            }

            public float GetFloorWithEmpty(float targetHeight)
            {
                if (!VectorUtility.InRange(Floor, Ceil, targetHeight)) { throw new Exception("TargetHeight is not in range!"); }

                var minWith = 0f;

                foreach (var box in Lower)
                {
                    var island = box;
                    if (VectorUtility.InRange(island.Pivot.y, island.GetMaxPos().y, targetHeight))
                    {
                        if (minWith < island.GetMaxPos().x) { minWith = island.GetMaxPos().x; }
                    }
                }



                return minWith;
            }
            public float GetCeilWithEmpty(float targetHeight)
            {
                if (!VectorUtility.InRange(Floor, Ceil, targetHeight)) throw new Exception("TargetHeight is not in range!");

                var maxWith = Width;

                foreach (var Box in Upper)
                {
                    var island = Box;
                    if (VectorUtility.InRange(island.Pivot.y, island.GetMaxPos().y, targetHeight))
                    {
                        if (island.GetMaxPos().x < maxWith) { maxWith = island.GetMaxPos().x; }
                    }
                }

                return maxWith;
            }
        }
    }
}