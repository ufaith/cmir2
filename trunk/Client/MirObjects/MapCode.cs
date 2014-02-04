using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Client.MirObjects
{
    public class CellInfo
    {
        public short BackIndex;
        public int BackImage;
        public short MiddleIndex;
        public short MiddleImage;
        public short FrontIndex;
        public short FrontImage;

        public byte DoorIndex;
        public byte DoorOffset;

        public byte AnimationFrame;
        public byte AnimationTick;

        public short TileAnimationIndex;
        public short TileAnimationTick;
        public short TileAnimationFrame;

        public byte Light;
        public byte Unknown;

        public List<MapObject> CellObjects;

        public void AddObject(MapObject ob)
        {
            if (CellObjects == null) CellObjects = new List<MapObject>();

            CellObjects.Insert(0, ob);
            Sort();
        }
        public void RemoveObject(MapObject ob)
        {
            CellObjects.Remove(ob);

            if (CellObjects.Count == 0) CellObjects = null;
            else Sort();
        }
        public void DrawObjects()
        {
            if (CellObjects == null) return;
            for (int i = 0; i < CellObjects.Count; i++)
                CellObjects[i].Draw();
        }

        public void Sort()
        {
            CellObjects.Sort(delegate(MapObject ob1, MapObject ob2)
            {
                if (ob1.Race == ObjectType.Item && ob2.Race != ObjectType.Item)
                    return -1;
                if (ob2.Race == ObjectType.Item && ob1.Race != ObjectType.Item)
                    return 1;
                if (ob1.Race == ObjectType.Spell && ob2.Race != ObjectType.Spell)
                    return -1;
                if (ob2.Race == ObjectType.Spell && ob1.Race != ObjectType.Spell)
                    return 1;

                int i = ob2.Dead.CompareTo(ob1.Dead);
                return i == 0 ? ob1.ObjectID.CompareTo(ob2.ObjectID) : i;
            });
        }
    }

    class MapReader
    {
        public int Width, Height;
        public CellInfo[,] MapCells;
        private string FileName;

        public MapReader(string FileName)
        {
            this.FileName = FileName;
            LoadMap();
        }

        public void LoadMap()
        {
            try
            {
                if (File.Exists(FileName))
                {
                    int offSet = 21;
                    byte[] fileBytes = File.ReadAllBytes(FileName);
                    int w = BitConverter.ToInt16(fileBytes, offSet);
                    offSet += 2;
                    int xor = BitConverter.ToInt16(fileBytes, offSet);
                    offSet += 2;
                    int h = BitConverter.ToInt16(fileBytes, offSet);
                    Width = w ^ xor;
                    Height = h ^ xor;
                    MapCells = new CellInfo[Width, Height];

                    offSet = 54;

                    for (int x = 0; x < Width; x++)
                        for (int y = 0; y < Height; y++)
                        {
                            MapCells[x, y] = new CellInfo
                                {
                                    BackIndex = 0,
                                    BackImage = (int)(BitConverter.ToInt32(fileBytes, offSet) ^ 0xAA38AA38),
                                    MiddleIndex = 1,
                                    MiddleImage = (short)(BitConverter.ToInt16(fileBytes, offSet += 4) ^ xor),
                                    FrontImage = (short)(BitConverter.ToInt16(fileBytes, offSet += 2) ^ xor),
                                    DoorIndex = fileBytes[offSet += 2],
                                    DoorOffset = fileBytes[++offSet],
                                    AnimationFrame = fileBytes[++offSet],
                                    AnimationTick = fileBytes[++offSet],
                                    FrontIndex = (short)(fileBytes[++offSet] + 2),
                                    Light = fileBytes[++offSet],
                                    Unknown = fileBytes[++offSet],
                                };
                            offSet++;
                        }
                }
                else
                {
                    Width = 1000;
                    Height = 1000;
                    MapCells = new CellInfo[Width, Height];

                    for (int x = 0; x < Width; x++)
                        for (int y = 0; y < Height; y++)
                        {
                            MapCells[x, y] = new CellInfo();
                        }
                }
            }
            catch (Exception ex)
            {
                if (Settings.LogErrors) CMain.SaveError(ex.ToString());
            }
        }

    }
}
