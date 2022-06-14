
namespace Udarverse.Editor
{
    using System.Collections.Generic;
    using Udarverse.Save;
    using UnityEditor;
    using UnityEditor.Events;
    using UnityEngine;

    public static class PlatformCreation
    {

        #region Constants
        private static readonly Vector3 TopOffset = new Vector3(0, .5f, 1.7f);
        private static readonly Vector3 TopRightOffset = new Vector3(1.45f, .5f, .85f);
        private static readonly Vector3 TopLeftOffset = new Vector3(-1.45f, .5f, .85f);
        private static readonly Vector3 DownOffset = new Vector3(0, .5f, -1.7f);
        private static readonly Vector3 DownRightOffset = new Vector3(1.45f, .5f, -.85f);
        private static readonly Vector3 DownLeftOffset = new Vector3(-1.45f, .5f, -.85f);

        private static readonly Quaternion TopRotation = Quaternion.Euler(90, 0, 0);
        private static readonly Quaternion TopRightRotation = Quaternion.Euler(90, 60f, 0);
        private static readonly Quaternion TopLeftRotation = Quaternion.Euler(90, -60f, 0);
        private static readonly Quaternion DownRotation = Quaternion.Euler(90, 180, 0);
        private static readonly Quaternion DownRightRotation = Quaternion.Euler(90, 120f, 0);
        private static readonly Quaternion DownLeftRotation = Quaternion.Euler(90, -120f, 0);
        #endregion

        public static PlatformEditorData PlatformEditorDataSC { get; private set; } = Resources.Load<PlatformEditorData>("_Data/Platforms/Platform_Editor_Data");

        public static Transform MapContainer { get; private set; }

        public static List<EntryPointPosition> PointsAvailableList { get; private set; } = new List<EntryPointPosition>();


        public static void Setup()
        {
            try
            {
                MapContainer = GameObject.Find("Map Container").transform;
            }
            catch
            {
                MapContainer = new GameObject("Map Container").transform;
            }
        }



        public static PlatformEntryPoint CreateNewEntryPoint(Platform newPlatform, EntryPointPosition entryPointPosition, Platform connectedPlatform)
        {
            var newEntryPoint = PrefabUtility.InstantiatePrefab(PlatformEditorDataSC.entryPointPfb, connectedPlatform.transform) as PlatformEntryPoint;
            Undo.RegisterCreatedObjectUndo(newEntryPoint.gameObject, "Created newEntryPoint");

            if (string.IsNullOrEmpty(connectedPlatform.GUID))
                connectedPlatform.CreateGUID();

            newEntryPoint.CreateGUID();
            newEntryPoint.transform.SetPositionAndRotation(GetEntryPosition(entryPointPosition, connectedPlatform.gameObject), GetEnrtyRotation(entryPointPosition));
            newEntryPoint.transform.name = "EntryPoint_" + entryPointPosition.ToString();
            EnrtyPointFixTransform(newEntryPoint.transform);
            newEntryPoint.SetPointTransistPosition(newPlatform.transform.position);
            FixObjectY(newEntryPoint.transform);
            UnityEventTools.AddBoolPersistentListener(newEntryPoint.OnAcomplished, newPlatform.gameObject.SetActive, true);

            return newEntryPoint;
        }
        public static Platform CreateNewPlatform(PlatformSC platformSC, EntryPointPosition entryPointPosition, Platform connectedPlatform, Quaternion rotation = default)
        {
            var newPlatform = PrefabUtility.InstantiatePrefab(platformSC.platformPfb, MapContainer) as Platform;
            Undo.RegisterCreatedObjectUndo(newPlatform.gameObject, "Created newPlatform");

            newPlatform.SetGUID(GUID.Generate().ToString());
            newPlatform.transform.position = GetNewPlatformPosition(entryPointPosition, connectedPlatform.gameObject);
            newPlatform.transform.rotation = rotation;
            newPlatform.HexagonColl.transform.eulerAngles -= Vector3.up * rotation.eulerAngles.y;
            newPlatform.transform.name = "Platform_" + platformSC.nameDisplay;

            return newPlatform;
        }
        public static Platform CreateNewPlatform(PlatformSC platformSC, Vector3 position,bool isBase=false)
        {
            var newPlatform = PrefabUtility.InstantiatePrefab(platformSC.platformPfb, MapContainer) as Platform;
            Undo.RegisterCreatedObjectUndo(newPlatform.gameObject, "Created newPlatform");

            newPlatform.SetGUID(GUID.Generate().ToString());
            newPlatform.transform.position = position;
            newPlatform.transform.name = "Platform_" + platformSC.nameDisplay;
            newPlatform.IsBase = isBase;
            return newPlatform;
        }

        public static PlatformEntryPoint CreateNewEntryPoint(Platform currentPlatform, GameObjectData entryData)
        {
            var newEntryPoint = PrefabUtility.InstantiatePrefab(PlatformEditorDataSC.entryPointPfb, currentPlatform.transform) as PlatformEntryPoint;
            newEntryPoint.transform.SetPositionAndRotation(entryData.position, entryData.rotation);
            newEntryPoint.transform.name = entryData.name;
            newEntryPoint.gameObject.SetActive(entryData.isActive);
            newEntryPoint.SetGUID(entryData.saveObjectGUID);
            return newEntryPoint;
        }
        public static Platform CreateNewPlatform(PlatformSC platformSC, GameObjectData platofrmData)
        {
            var newPlatform = PrefabUtility.InstantiatePrefab(platformSC.platformPfb, MapContainer) as Platform;
            Undo.RegisterCreatedObjectUndo(newPlatform.gameObject, "Created newPlatform");
            newPlatform.transform.SetPositionAndRotation(platofrmData.position, platofrmData.rotation);
            newPlatform.transform.localScale = platofrmData.localScale;
            newPlatform.transform.name = platofrmData.name;
            newPlatform.gameObject.SetActive(platofrmData.isActive);
            newPlatform.SetGUID(platofrmData.saveObjectGUID);

            return newPlatform;
        }

        public static void SetPointsAvaliableList(GameObject platform)
        {
            LayerMask mask = LayerMask.GetMask("Platform", "Unit");


            PointsAvailableList.Clear();
            var position = GetNewPlatformPosition(EntryPointPosition.Top, platform);
            if (!Physics.CheckBox(position, Vector3.one, Quaternion.identity, mask))
                PointsAvailableList.Add(EntryPointPosition.Top);

            position = GetNewPlatformPosition(EntryPointPosition.TopRight, platform);
            if (!Physics.CheckBox(position, Vector3.one, Quaternion.identity, mask))
                PointsAvailableList.Add(EntryPointPosition.TopRight);

            position = GetNewPlatformPosition(EntryPointPosition.TopLeft, platform);
            if (!Physics.CheckBox(position, Vector3.one, Quaternion.identity, mask))
                PointsAvailableList.Add(EntryPointPosition.TopLeft);

            position = GetNewPlatformPosition(EntryPointPosition.Down, platform);
            if (!Physics.CheckBox(position, Vector3.one, Quaternion.identity, mask))
                PointsAvailableList.Add(EntryPointPosition.Down);

            position = GetNewPlatformPosition(EntryPointPosition.DownRight, platform);
            if (!Physics.CheckBox(position, Vector3.one, Quaternion.identity, mask))
                PointsAvailableList.Add(EntryPointPosition.DownRight);

            position = GetNewPlatformPosition(EntryPointPosition.DownLeft, platform);
            if (!Physics.CheckBox(position, Vector3.one, Quaternion.identity, mask))
                PointsAvailableList.Add(EntryPointPosition.DownLeft);
        }
        public static Vector3 GetEntryPosition(EntryPointPosition entryPoint, GameObject selection)
        {
            switch (entryPoint)
            {
                case EntryPointPosition.Top:
                    {
                        return CalculateFullEntryPosition(TopOffset, selection);
                    }
                case EntryPointPosition.TopRight:
                    {

                        return CalculateFullEntryPosition(TopRightOffset, selection);
                    }
                case EntryPointPosition.TopLeft:
                    {
                        return CalculateFullEntryPosition(TopLeftOffset, selection);
                    }
                case EntryPointPosition.Down:
                    {
                        return CalculateFullEntryPosition(DownOffset, selection);
                    }
                case EntryPointPosition.DownRight:
                    {
                        return CalculateFullEntryPosition(DownRightOffset, selection);
                    }
                case EntryPointPosition.DownLeft:
                    {
                        return CalculateFullEntryPosition(DownLeftOffset, selection);
                    }
            }
            return Vector3.zero;
        }
        public static Quaternion GetEnrtyRotation(EntryPointPosition entryPoint)
        {
            switch (entryPoint)
            {
                case EntryPointPosition.Top:
                    {
                        return TopRotation;
                    }
                case EntryPointPosition.TopRight:
                    {

                        return TopRightRotation;
                    }
                case EntryPointPosition.TopLeft:
                    {
                        return TopLeftRotation;
                    }
                case EntryPointPosition.Down:
                    {
                        return DownRotation;
                    }
                case EntryPointPosition.DownRight:
                    {
                        return DownRightRotation;
                    }
                case EntryPointPosition.DownLeft:
                    {
                        return DownLeftRotation;
                    }
            }
            return Quaternion.identity;
        }

        public static void EnrtyPointFixTransform(Transform transform)
        {
            Vector3 euler = transform.eulerAngles;
            euler.x = -90f;
            transform.eulerAngles = euler;

            Vector3 position = transform.position;
            position.y += 0.05f;
            position += transform.up * 0.3f;
            transform.position = position;
        }
        public static Vector3 GetNewPlatformPosition(EntryPointPosition entryPointPosition, GameObject currentPlatform)
        {
            Collider collider = currentPlatform.GetComponent<Platform>().HexagonColl;
            Vector3 newPos = currentPlatform.transform.position;
            switch (entryPointPosition)
            {
                case EntryPointPosition.Top:
                    {
                        newPos.z += collider.bounds.size.z;
                        return newPos;
                    }
                case EntryPointPosition.TopRight:
                    {
                        newPos.z += collider.bounds.size.z / 2;
                        newPos.x += collider.bounds.size.x * .75f;
                        return newPos;
                    }
                case EntryPointPosition.TopLeft:
                    {
                        newPos.z += collider.bounds.size.z / 2;
                        newPos.x -= collider.bounds.size.x * .75f;
                        return newPos;
                    }
                case EntryPointPosition.Down:
                    {
                        newPos.z -= collider.bounds.size.z;
                        return newPos;
                    }
                case EntryPointPosition.DownRight:
                    {
                        newPos.z -= collider.bounds.size.z / 2;
                        newPos.x += collider.bounds.size.x * .75f;
                        return newPos;
                    }
                case EntryPointPosition.DownLeft:
                    {
                        newPos.z -= collider.bounds.size.z / 2;
                        newPos.x -= collider.bounds.size.x * .75f;
                        return newPos;
                    }
            }
            return Vector3.zero;
        }
        private static Vector3 CalculateFullEntryPosition(Vector3 offset, GameObject selectionObject)
        {
            if (selectionObject == null)
                return Vector3.zero;
            float scale = selectionObject.transform.localScale.x;
            return selectionObject.transform.position + offset * scale;
        }

        public static void FixObjectY(Platform platform)
        {
            foreach (Transform child in platform.transform)
            {
                if (child.name.Equals("GFX"))
                    continue;
                FixObjectY(child);
            }
        }
        private static void FixObjectY(Transform transform)
        {
            var newPos = transform.position;
            newPos.y += 1000f;
            LayerMask mask = LayerMask.GetMask("Platform");

            if (Physics.Raycast(newPos, Vector3.down, out RaycastHit hitInfo, Mathf.Infinity, mask))
            {
                var pos = hitInfo.point;
                pos.y += 0.025f;

                transform.position = pos;
            }

        }
    }

    public enum EntryPointPosition
    {
        Top,
        TopRight,
        TopLeft,
        Down,
        DownRight,
        DownLeft
    }
}