namespace SkelConverter
{
    internal class Worker
    {
        private readonly SkeletonHappy s;
        private bool non;

        public Worker(SkeletonHappy s)
        {
            this.s = s;
        }

        public void Work()
        {
            s.String(); // hash
            s.r.ReadString();
            s.w.Write("3.8.80"); // version
            s.w.Write(0f); // x
            s.w.Write(0f); // y
            s.Do("ff"); // width, height
            non = s.r.ReadBoolean();
            s.w.Write(non);
            if (non)
            {
                s.Do("fs"); // fps, imgPath
                s.w.Write(""); // default: audioPath
            }

            s.w.SwitchToTempStream(); // from here: prepared to write string dict

            s.Foreach("Bones", i =>
            {
                s.String(); // name
                if (i != 0) s.PInt(); // parent
                s.Do("ffff ffff i"); // rotation, x, y, scaleX, scaleY, shearX, shearY, length, transformMode
                s.w.Write(false); // default: !!!skinRequired
                if (non) s.Int(); // Small bug :)   (previous: PInt)
            });

            s.Foreach("Slots", i =>
            {
                s.Do("si"); // name, bone
                s.Int(); // color
                s.Int(); // dark color
                s.Do("Si"); // attachmentName, blendMode
            });

            s.Foreach("IKs", i =>
            {
                s.Do("si"); // name, order
                s.w.Write(false); // default: !!!skinRequired
                s.Foreach(i => s.PInt()); // bones
                s.Do("if"); // target, mix
                s.w.Write(0f); // default: !!!softness
                s.SByte(); // bendDirection
                s.w.Write(false); // default: !!!compress
                s.w.Write(false); // default: !!!stretch
                s.w.Write(false); // default: !!!uniform
            });

            s.Foreach("Transforms", i =>
            {
                s.Do("si"); // name, order
                s.w.Write(false); // default: !!!skinRequired
                s.Foreach(i => s.PInt()); // bones
                s.PInt(); // target
                s.Do("00 ffff ffff ff"); // local, relative, offsetRotation, offsetX, offsetY, offsetScaleX, offsetScaleY, offsetShearY, rotateMix, translateMix, scaleMix, shearMix
            });

            s.Foreach("Paths", i =>
            {
                s.Do("si"); // name, order
                s.w.Write(false); // default: !!!skinRequired
                s.Foreach(i => s.PInt()); // bones
                s.Do("iiii ffff f"); // offsetRotation, offsetX, offsetY, offsetScaleX, offsetScaleY, offsetShearY, rotateMix, translateMix, scaleMix, shearMix
            });

            s.Logger("Default skin:");
            s.level += 1;
            ReadSkin(true);
            s.level -= 1;
            s.Foreach("Skins", _ => ReadSkin(false));

            s.Foreach("Events", _ =>
            {
                s.Do("Sifs");
                s.w.Write(null as string); // AudioPath
            });

            s.Foreach("Animations", _ => ReadAnimation());

            s.w.WriteTmp();
        }

        private void ReadAnimation()
        {
            s.String(); // name

            s.Foreach("Slots", _ =>
            {
                s.Log("Slot index", s.PInt());
                s.Foreach(_ =>
                {
                    var type = s.Byte();
                    s.Foreach((i, n) =>
                    {
                        switch (type)
                        {
                            case 0:
                                s.Do("fS");
                                break;

                            case 1:
                                s.Do("fI");
                                if (i < n - 1) ReadCurve();
                                break;

                            case 2:
                                s.Do("fII");
                                if (i < n - 1) ReadCurve();
                                break;

                            default:
                                throw new Exception();
                        }
                    });
                });
            });

            s.Foreach("Bones", _ =>
            {
                s.Log("Bone index", s.PInt());
                s.Foreach(_ =>
                {
                    var type = s.Byte();
                    s.Foreach((i, n) =>
                    {
                        switch (type)
                        {
                            case 0: s.Do("ff"); break;
                            case 1:
                            case 2:
                            case 3: s.Do("fff"); break;
                            default: throw new Exception();
                        }
                        if (i < n - 1) ReadCurve();
                    });
                });
            });

            s.Foreach("IKs", _ =>
            {
                s.Log("Index", s.PInt());
                s.Foreach((i, n) =>
                {
                    s.Do("ff");
                    s.w.Write((float)0);
                    s.SByte();
                    s.w.Write(false);
                    s.w.Write(false); // default: !!!
                    if (i < n - 1) ReadCurve();
                });
            });

            //s.Foreach("Paths", _ => {
            //    s.Log("Index", s.PInt());
            //    s.Foreach((i, n) => {
            //        s.Do("fffff");
            //        if (i < n - 1) ReadCurve();
            //    });
            //});

            s.Foreach("Transforms", _ =>
            {
                s.Log("Index", s.PInt());
                s.Foreach((i, n) =>
                {
                    s.Do("fffff");
                    if (i < n - 1) ReadCurve();
                });
            });

            s.Foreach("Paths", _ =>
            {
                s.Log("Index", s.PInt());
                s.Foreach(_ =>
                {
                    var type = s.Byte();
                    s.Foreach((i, n) =>
                    {
                        switch (type)
                        {
                            case 0:
                            case 1: s.Do("ff"); break;
                            case 2: s.Do("fff"); break;
                            default: throw new Exception();
                        }
                        if (i < n - 1) ReadCurve();
                    });
                });
            });

            s.Foreach("Deform", _ =>
            {
                s.Log("Skin", s.PInt());
                s.Foreach(_ =>
                {
                    s.Log("Slot index", s.PInt());
                    s.Foreach(_ =>
                    {
                        s.RefString();
                        s.Foreach((i, n) =>
                        {
                            s.Float(); // time
                            int end = s.PInt();
                            if (end != 0)
                            {
                                s.PInt();
                                s.Foreach(end, _ => s.Float());
                            }
                            if (i < n - 1) ReadCurve();
                        });
                    });
                });
            });

            s.Foreach("Draworder", _ =>
            {
                s.Float();
                s.Foreach(_ => s.Do("ii"));
            });

            s.Foreach("Event", _ =>
            {
                s.Float();
                s.PInt();
                s.PInt(false);
                s.Float();
                bool v = s.r.ReadBoolean();
                s.w.Write(v);
                if (v) s.String();
            });
        }

        private void ReadCurve()
        {
            if (s.Byte() == 2) s.Do("ffff");
        }

        private void ReadSkin(bool defaultSkin)
        {
            int slotCount;
            if (defaultSkin)
            {
                slotCount = s.PInt();
            }
            else
            {
                s.RefString(); // name
                s.w.WriteOptimizedPositiveInt(0);
                s.w.WriteOptimizedPositiveInt(0);
                s.w.WriteOptimizedPositiveInt(0);
                s.w.WriteOptimizedPositiveInt(0);
                slotCount = s.PInt();
            }
            s.Foreach("Slots", slotCount, _ =>
            {
                s.Log("Slot index", s.PInt());
                s.Foreach("Attachments", _ =>
                {
                    s.RefString();
                    ReadAttachment();
                });
            });
        }

        private void ReadAttachment()
        {
            s.RefString();
            int vertexCount;
            switch (s.Byte())
            {
                case 0: // Region
                    s.Do("S ffff fff");
                    s.Int(); // color
                    break;

                case 1: // Boundingbox
                    ReadVertices(s.PInt());
                    if (non) s.Int();
                    break;

                case 2:
                    s.RefString(); // path
                    s.Int(); // color
                    vertexCount = s.PInt();
                    ReadFloatArray(vertexCount << 1);
                    ReadShortArray();
                    ReadVertices(vertexCount);
                    s.PInt();
                    if (non)
                    {
                        ReadShortArray();
                        s.Float();
                        s.Float();
                    }
                    break;

                case 3:
                    s.Do("SiSS0");
                    if (non) s.Do("ff");
                    break;

                case 4:
                    s.Do("00");
                    vertexCount = s.PInt();
                    ReadVertices(vertexCount);
                    for (int i = 0, n = vertexCount / 3; i < n; ++i) s.Float();
                    if (non) s.Int();
                    break;

                case 5:
                    s.Do("fff"); // rotation, x, y
                    if (non) s.Int(); // color
                    break;

                case 6:
                    s.PInt(); // endSlotIndex
                    vertexCount = s.PInt();
                    ReadVertices(vertexCount);
                    if (non) s.Int();
                    break;

                default:
                    throw new Exception();
            }
        }

        private void ReadVertices(int vertexCount)
        {
            bool v = s.r.ReadBoolean();
            s.w.Write(v);
            if (!v)
            {
                ReadFloatArray(vertexCount << 1);
                return;
            }
            s.Foreach(vertexCount, _ => s.Foreach(_ => s.Do("ifff")));
        }

        private void ReadFloatArray(int n) => s.Foreach(n, _ => s.Float());

        private void ReadShortArray() => s.Foreach(_ => s.Do("bb"));
    }
}