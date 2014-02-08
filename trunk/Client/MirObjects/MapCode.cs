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
        private byte[] Bytes;
        private bool loaded;
        
        public MapReader(string FileName)
        {
            this.FileName = FileName;
            initiate();
        }

        private void initiate()
        {
            if (File.Exists(FileName))
            {
                Bytes = File.ReadAllBytes(FileName);
                loaded = true;
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
                return;
            }
            //wemade mir3 maps have no title they just start with blank bytes
            if (Bytes[0] == 0)
            {
                //load wemade mir3 map
                LoadMapType5();
                return;
            }
            //shanda mir3 maps start with title: (C) SNDA, MIR3.
            if ((Bytes[0] == 0x0F) && (Bytes[5] == 0x53) && (Bytes[14] == 0x33))
            {
                //load shanda mir3 map
                //LoadMapType6();
                return;
            }
            //wemades antihack map (laby maps) title start with: Mir2 AntiHack
            if ((Bytes[0] == 0x15) && (Bytes[4] == 0x32) && (Bytes[6] == 0x41) && (Bytes[19] == 0x31))
            {
                //load wemade mir2 antihack map (aka laby maps)
                //LoadMapType4();
                return;
            }
            //wemades 2010 map format i guess title starts with: Map 2010 Ver 1.0
            if ((Bytes[0] == 0x10) && (Bytes[2] == 0x61) && (Bytes[7] == 0x31) && (Bytes[14] == 0x31))
            {
                LoadMapType1();
                return;
            }
            //shanda's 2012 format and one of shandas(wemades) older formats share same header info, only difference is the filesize
            if ((Bytes[4] == 0x0F) && (Bytes[18] == 0x0D) && (Bytes[19] == 0x0A))
            {
                int W = Bytes[0] + (Bytes[1] << 8);
                int H = Bytes[2] + (Bytes[3] << 8);
                if (Bytes.Length > (52 + (W*H*14)))
                {
                    //load shanda 2012 format
                    //LoadMapType3();
                    return;
                }
                else
                {
                     //load format other format
                    //LoadMapType2();
                    return;
                }
            }

            //3/4 heroes map format (myth/lifcos i guess)
            if ((Bytes[0] == 0x0D) && (Bytes[1] == 0x4C) && (Bytes[7] == 0x20) && (Bytes[11] == 0x6D))
            {
                //load 3/4 heroes map format
                //LoadMapType7();
                return;
            }
            //if it's none of the above load the default old school format
            //LoadMapType0();

        }

        private void LoadMapType1()
        {
            try
            {
                int offSet = 21;
                   
                int w = BitConverter.ToInt16(Bytes, offSet);
                offSet += 2;
                int xor = BitConverter.ToInt16(Bytes, offSet);
                offSet += 2;
                int h = BitConverter.ToInt16(Bytes, offSet);
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
                                BackImage = (int)(BitConverter.ToInt32(Bytes, offSet) ^ 0xAA38AA38),
                                MiddleIndex = 1,
                                MiddleImage = (short)(BitConverter.ToInt16(Bytes, offSet += 4) ^ xor),
                                FrontImage = (short)(BitConverter.ToInt16(Bytes, offSet += 2) ^ xor),
                                DoorIndex = Bytes[offSet += 2],
                                DoorOffset = Bytes[++offSet],
                                AnimationFrame = Bytes[++offSet],
                                AnimationTick = Bytes[++offSet],
                                FrontIndex = (short)(Bytes[++offSet] + 2),
                                Light = Bytes[++offSet],
                                Unknown = Bytes[++offSet],
                            };
                        offSet++;
                    }
            }
            catch (Exception ex)
            {
                if (Settings.LogErrors) CMain.SaveError(ex.ToString());
            }
        }

        private void LoadMapType5()
        {
            try
            {
                byte flag = 0;
                int offset = 20;
                short Attribute = (short)(BitConverter.ToInt16(Bytes,offset));
                Width = (int)(BitConverter.ToInt16(Bytes,offset+=2));
                Height = (int)(BitConverter.ToInt16(Bytes, offset += 2));
                //ignoring eventfile and fogcolor for now (seems unused in maps i checked)
                offset = 28;
                //initiate all cells
                MapCells = new CellInfo[Width, Height];
                for (int x = 0; x < Width; x++)
                    for (int y = 0; y < Height; y++)
                        MapCells[x, y] = new CellInfo();
                //read all back tiles
                for (int x = 0; x < (Width/2); x++)
                    for (int y = 0; y < (Height/2); y++)
                    {
                        for (int i = 0; i < 4; i++)
                        {//todo: check if my math is accurate here:p
                            //MapCells[(x*2) + (i % 2), (y*2) + (i / 2)].BackIndex = (short)(Bytes[offset] > 0 ? Bytes[offset] + 100 : 0);
                            MapCells[(x * 2) + (i % 2), (y * 2) + (i / 2)].BackIndex = (short)(Bytes[offset]+100);
                            MapCells[(x*2) + (i % 2), (y*2) + (i / 2)].BackImage = (int)(BitConverter.ToInt16(Bytes, offset + 1)+1);
                        }
                        offset += 3;
                    }
                //read rest of data
                offset = 28 + (3 * (Width /2) * (Height / 2));
                for (int x = 0; x < Width; x++)
                    for (int y = 0; y < Height; y++)
                    {
                        flag = Bytes[offset++];
                        offset+=2;//aniframe > aniframefr
                        MapCells[x,y].FrontIndex = (short)(Bytes[offset] > 0 ? Bytes[offset] + 100 : 0);
                        offset++;
                        MapCells[x,y].MiddleIndex = (short)(Bytes[offset] > 0 ? Bytes[offset] + 100 : 0);
                        offset++;
                        MapCells[x,y].MiddleImage = (short)(BitConverter.ToInt16(Bytes,offset)+1);
                        offset += 2;
                        MapCells[x, y].FrontImage = (short)(BitConverter.ToInt16(Bytes, offset)+1);

                        offset += 2;
                        offset += 5;//mir3 maps dont have doors so dont bother reading the info, also not using light info atm
                        if (flag == 1) MapCells[x, y].BackImage |= 0x20000000;
                        if (flag == 2) MapCells[x, y].FrontImage = (short)(MapCells[x, y].FrontImage | 0x8000);
                    }
            }
            catch (Exception ex)
            {
                if (Settings.LogErrors) CMain.SaveError(ex.ToString());
            }
        }

    }
}
