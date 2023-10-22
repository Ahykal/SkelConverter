﻿using Spine;
using System.Collections;

namespace SkelConverter
{
    public class SkelDataConverter
    {
        public static Dictionary<string, object> FromSkeletonData(SkeletonData skeletonData)
        {
            var root = new Dictionary<String, Object>();

            //float scale = 1;
            //var root = new JObject();

            //skeleton
            //var skeleton=new JObject();
            //skeleton["hash"] = skeletonData.Hash;
            //skeleton["spine"] = skeletonData.Version;
            //skeleton["width"] = skeletonData.Width;
            //skeleton["height"] = skeletonData.Height;
            //skeleton["fps"] = skeletonData.Fps;
            //skeleton["images"] = skeletonData.ImagesPath;
            //root["skeleton"] = skeleton;

            var sd = new Dictionary<string, object>();
            sd["hash"] = skeletonData.Hash;
            sd["spine"] = skeletonData.Version;
            sd["width"] = skeletonData.Width;
            sd["height"] = skeletonData.Height;
            sd["fps"] = skeletonData.Fps;
            sd["image"] = skeletonData.ImagesPath;
            root["skeleton"] = sd;
            //bones
            //var bones=new JArray();
            //foreach (var boneData in skeletonData.Bones)
            //{
            //    var bone = new JObject();
            //    bone["name"]=boneData.Name;
            //    bone["parent"] = boneData.Parent?.Name??null;
            //    bone["length"] = boneData.Length;
            //    bone["x"] = boneData.X;
            //    bone["y"] = boneData.Y;
            //    bone["rotation"] = boneData.Rotation;
            //    bone["scaleX"] = boneData.ScaleX;
            //    bone["scaleY"] = boneData.ScaleY;
            //    bone["shearX"] = boneData.ShearX;
            //    bone["shearY"] = boneData.ShearY;
            //    bone["transform"]=boneData.TransformMode.ToString().ToLower();
            //    bones.Add(bone);
            //}
            //root["bones"] = bones;

            List<Object> bn = new List<object>();
            foreach (BoneData data in skeletonData.Bones)
            {
                Dictionary<String, Object> bm = new Dictionary<string, object>();
                bm["name"] = data.Name;
                if (data.Parent != null)
                    bm["parent"] = data.Parent.Name;
                bm["length"] = data.Length;
                bm["x"] = data.X;
                bm["y"] = data.Y;
                bm["rotation"] = data.Rotation;
                bm["scaleX"] = data.ScaleX;
                bm["scaleY"] = data.ScaleY;
                bm["shearX"] = data.ShearX;
                bm["shearY"] = data.ShearY;
                bm["transform"] = data.TransformMode.ToString().ToLower();
                // Likely is aborlished,
                //bm["inheritRotation"] = data.InheritRotation;
                //bm["inheritScale"] = data.InheritScale;

                bn.Add(bm);
            }
            root["bones"] = bn;

            //slots
            //var slots = new JArray();
            //foreach (var slotData in skeletonData.Slots)
            //{
            //    var slot = new JObject();
            //    slot["name"]=slotData.Name;
            //    slot["bone"] = slotData.BoneData.Name;
            //    slot["attachment"] = slotData.AttachmentName;
            //    slot["blend"] = slotData.BlendMode.ToString().ToLower();
            //    slots.Add(slot);
            //}
            //root["slots"] = slots;
            List<Object> slots = new List<object>();
            foreach (SlotData data in skeletonData.Slots)
            {
                Dictionary<string, object> sm = new Dictionary<string, object>();
                sm["name"] = data.Name;
                sm["bone"] = data.BoneData.Name;
                sm["color"] = Util.ColorToHex(data.R, data.G, data.B, data.A);//ignore color because toom uch work

                if (data.HasSecondColor)
                {
                    sm["dark"] = Util.ColorToHex(data.R2, data.G2, data.B2);
                }
                sm["attachment"] = data.AttachmentName;
                //if (data.BlendMode != 0)
                //sm["blend"] = Enum.GetName(typeof(BlendMode), data.BlendMode).ToLower();
                sm["blend"] = data.BlendMode.ToString().ToLower();

                slots.Add(sm);
            }
            root["slots"] = slots;

            // IK constraints.
            //var iks=new JArray();
            //foreach (var ikData in skeletonData.IkConstraints)
            //{
            //    var ik=new JObject();
            //    ik["name"] = ikData.Name;
            //    ik["order"] = ikData.Order;
            //    ik["bones"]=new JArray(ikData.Bones.Select(x => x.Name));
            //    ik["target"] = ikData.Target.Name;
            //    ik["bendPositive"]=ikData.BendDirection==1?true:false;
            //    ik["mix"] = ikData.Mix;
            //    iks.Add(ik);
            //}
            //root["ik"] = iks;
            List<object> ik = new List<object>();
            foreach (IkConstraintData data in skeletonData.IkConstraints)
            {
                Dictionary<string, object> cm = new Dictionary<string, object>();
                cm["name"] = data.Name;
                List<string> bnames = new List<string>();
                foreach (var bone in data.Bones)
                {
                    bnames.Add(bone.Name);
                }
                cm["bones"] = bnames;
                cm["target"] = data.Target.Name;
                cm["mix"] = data.Mix;
                ik.Add(cm);
            }
            root["ik"] = ik;

            // Transform constraints.
            //var transforms = new JArray();
            //foreach (var transformData in skeletonData.TransformConstraints)
            //{
            //    var transform = new JObject();

            //    transform["name"] = transformData.Name;
            //    transform["order"] = transformData.Order;
            //    transform["bones"] = new JArray(transformData.Bones.Select(x => x.Name));
            //    transform["target"] = transformData.Target.Name;
            //    transform["local"] = transformData.Local;
            //    transform["relative"] =transformData.Relative;
            //    transform["rotation"] = transformData.offsetRotation;
            //    transform["x"] = transformData.offsetX/scale;
            //    transform["y"] = transformData.offsetY/scale;
            //    transform["scaleX"] = transformData.offsetScaleX;
            //    transform["scaleY"] = transformData.offsetScaleY;
            //    transform["shearY"] = transformData.offsetShearY;
            //    transform["rotateMix"] = transformData.rotateMix;
            //    transform["translateMix"] = transformData.translateMix;
            //    transform["scaleMix"] = transformData.scaleMix;
            //    transform["shearMix"] = transformData.shearMix;
            //    transforms.Add(transform);
            //}
            //root["transform"]=transforms;

            List<object> transform = new List<object>();
            foreach (TransformConstraintData data in skeletonData.TransformConstraints)
            {
                Dictionary<string, object> cm = new Dictionary<string, object>();
                cm["name"] = data.Name;
                List<Object> bones = new List<object>();
                //get names of all bones for this transform
                foreach (var bone in data.Bones)
                    bones.Add(bone.Name);
                cm["bones"] = bones;
                cm["target"] = data.Target.Name;
                cm["rotation"] = data.OffsetRotation;
                cm["x"] = data.OffsetX;
                cm["y"] = data.OffsetY;
                cm["scaleX"] = data.OffsetScaleX;
                cm["scaleY"] = data.OffsetScaleY;
                cm["shearY"] = data.OffsetShearY;
                cm["rotateMix"] = data.RotateMix;
                cm["translateMix"] = data.TranslateMix;
                cm["scaleMix"] = data.ScaleMix;
                cm["shearMix"] = data.ShearMix;

                transform.Add(cm);
            }
            root["transform"] = transform;

            //Path
            //var paths=new JArray();
            //foreach (var pathData in skeletonData.PathConstraints)
            //{
            //    var path = new JObject();
            //    path["name"] = pathData.Name;
            //    path["order"]=pathData.Order;
            //    path["bones"] = new JArray(pathData.Bones.Select(x => x.Name));
            //    path["target"] = pathData.Target.Name;
            //    path["positionMode"] = pathData.positionMode.ToString().ToLower();
            //    path["spacingMode"]=pathData.spacingMode.ToString().ToLower();
            //    path["rotateMode"]=pathData.rotateMode.ToString().ToLower();
            //    path["rotation"] = pathData.offsetRotation;
            //    path["position"] = pathData.Position;
            //    path["spacing"] = pathData.Spacing;
            //    path["rotateMix"] = pathData.RotateMix;
            //    path["translateMix"] = pathData.TranslateMix;
            //    paths.Add(path);
            //}
            //root["path"] = paths;
            List<object> path = new List<object>();
            foreach (PathConstraintData data in skeletonData.PathConstraints)
            {
                Dictionary<string, object> cm = new Dictionary<string, object>();
                cm["name"] = data.Name;
                List<string> bones = new List<string>();
                foreach (var bone in data.Bones)
                    bones.Add(bone.Name);
                cm["bones"] = bones;
                cm["target"] = data.Target.Name;
                //cm["positionMode"] = Enum.GetName(typeof(PositionMode), data.PositionMode).ToLower();
                //cm["spacingMode"] = Enum.GetName(typeof(SpacingMode), data.SpacingMode).ToLower();
                //cm["rotateMode"] = Enum.GetName(typeof(RotateMode), data.RotateMode).ToLower();
                cm["positionMode"] = data.PositionMode.ToString().ToLower();
                cm["spacingMode"] = data.SpacingMode.ToString().ToLower();
                cm["rotateMode"] = data.RotateMode.ToString().ToLower();
                cm["rotation"] = data.OffsetRotation;
                cm["position"] = data.Position;
                cm["spacing"] = data.Spacing;
                cm["rotateMix"] = data.RotateMix;
                cm["translateMix"] = data.TranslateMix;

                path.Add(cm);
            }
            root["path"] = path;

            //"skins":{
            //  skin.name: {}
            //      sm:{}
            //          attachment:{...}
            List<string> skinName = new List<string>();
            Dictionary<string, object> skins = new Dictionary<string, object>();
            foreach (Skin skin in skeletonData.Skins)
            {
                Dictionary<string, Dictionary<string, object>> sm = new Dictionary<string, Dictionary<string, object>>();
                foreach (var at in skin.Attachments)
                {
                    string slotName = skeletonData.Slots.Items[at.Key.slotIndex].Name;
                    if (!sm.ContainsKey(slotName))
                        sm[slotName] = new Dictionary<string, object>();
                    string name = at.Key.name;
                    //Console.WriteLine(at.Key.name);
                    if (name.Contains('/'))
                        name = name.Substring(name.LastIndexOf('/') + 1);
                    name = name.Replace('-', ' ');
                    sm[slotName][name] = fromAttachment(at.Value);
                }
                skins[skin.Name] = sm;
                skinName.Add(skin.Name);
            }
            root["skins"] = skins;
            root["skinsName"] = skinName;

            Dictionary<string, object> events = new Dictionary<string, object>();
            foreach (EventData data in skeletonData.Events)
            {
                Dictionary<string, object> em = new Dictionary<string, object>();
                em["int"] = data.Int;
                em["float"] = data.Float;
                em["string"] = data.String;

                events[data.Name] = em;
            }
            root["events"] = events;

            //animations
            Dictionary<string, object> animations = new Dictionary<string, object>();
            foreach (Animation data in skeletonData.Animations)
            {
                animations[data.Name] = fromAnimation(data, skeletonData);
            }
            root["animations"] = animations;

            return root;
        }

        public static Dictionary<string, object> fromAttachment(Attachment data)
        {
            float scale = 1;
            Dictionary<string, object> fa = new Dictionary<string, object>();

            switch (data)
            {
                case RegionAttachment region:
                    fa["type"] = "region";
                    fa["name"] = region.Path;
                    fa["path"] = region.Path;
                    fa["x"] = region.X / scale;
                    fa["y"] = region.Y / scale;
                    fa["scaleX"] = region.ScaleX;
                    fa["scaleY"] = region.ScaleY;
                    fa["rotation"] = region.Rotation;
                    fa["width"] = region.Width / scale;
                    fa["height"] = region.Height / scale;
                    fa["color"] = Util.ColorToHex(region.R, region.G, region.B, region.A);
                    //fa["color"] //not gonna bother with color
                    break;

                case BoundingBoxAttachment box:
                    fa["type"] = "boundingbox";
                    fa["vertexCount"] = box.Vertices.Length;
                    fa["vertices"] = box.Vertices;
                    break;

                case MeshAttachment mesh:
                    fa["type"] = "mesh";
                    fa["name"] = mesh.Path;
                    fa["path"] = mesh.Path;
                    fa["width"] = mesh.Width;
                    fa["height"] = mesh.Height;
                    fa["uvs"] = mesh.RegionUVs;
                    fa["triangles"] = mesh.Triangles;
                    if (mesh.Vertices.Length > mesh.UVs.Length)
                    {
                        List<float> arr = new List<float>();
                        int[] bones = mesh.Bones;
                        int i = 0;
                        int z = 0;
                        //# bones, bone#0, x,y,weight,bone#1,...
                        while (i < bones.Length)
                        {
                            int numBones = bones[i++];
                            arr.Add(numBones);
                            for (int k = 0; k < numBones; k++)
                            {
                                arr.Add(bones[i++]);
                                arr.Add(mesh.Vertices[z++]); //x pos
                                arr.Add(mesh.Vertices[z++]); //y pos
                                arr.Add(mesh.Vertices[z++]); //weight
                            }
                        }
                        fa["vertices"] = arr;
                    }
                    else
                        fa["vertices"] = mesh.Vertices;
                    fa["hull"] = mesh.HullLength / 2;
                    fa["edges"] = mesh.Edges;
                    break;

                case PathAttachment path:
                    fa["type"] = "path";
                    fa["closed"] = path.Closed;
                    fa["constantSpeed"] = path.ConstantSpeed;
                    int numVerts = path.Lengths.Length * 3;
                    fa["vertexCount"] = numVerts;
                    fa["lengths"] = path.Lengths;
                    if (path.Vertices.Length > numVerts * 2)
                    {
                        List<float> arr = new List<float>();
                        int[] bones = path.Bones;
                        int i = 0;
                        int z = 0;
                        //# bones, bone#0, x,y,weight,bone#1,...
                        while (i < bones.Length)
                        {
                            int numBones = bones[i++];
                            arr.Add(numBones);
                            for (int k = 0; k < numBones; k++)
                            {
                                arr.Add(bones[i++]);
                                arr.Add(path.Vertices[z++]); //x pos
                                arr.Add(path.Vertices[z++]); //y pos
                                arr.Add(path.Vertices[z++]); //weight
                            }
                        }
                        fa["vertices"] = arr;
                    }
                    else
                        fa["vertices"] = path.Vertices;
                    break;

                default:
                    break;
            }

            return fa;
        }

        private static Dictionary<string, object> fromAnimation(Animation animation, SkeletonData skeletonData)
        {
            Dictionary<string, object> anim = new Dictionary<string, object>();
            Dictionary<string, Dictionary<string, List<object>>> bones = new Dictionary<string, Dictionary<string, List<object>>>();
            Dictionary<string, List<object>> ik = new Dictionary<string, List<object>>();
            Dictionary<string, Dictionary<string, List<object>>> paths = new Dictionary<string, Dictionary<string, List<object>>>();
            Dictionary<string, Dictionary<string, List<object>>> slots = new Dictionary<string, Dictionary<string, List<object>>>();
            Dictionary<string, Dictionary<string, List<object>>> transform = new Dictionary<string, Dictionary<string, List<object>>>();
            Dictionary<string, Dictionary<string, List<object>>> events = new Dictionary<string, Dictionary<string, List<object>>>();
            Dictionary<string, Dictionary<string, Dictionary<string, List<object>>>> deform = new Dictionary<string, Dictionary<string, Dictionary<string, List<object>>>>();
            List<Dictionary<string, object>> drawOrder = new List<Dictionary<string, object>>();

            List<object> tl;
            Dictionary<string, object> tmp;

            foreach (var timeline in animation.Timelines)
            {
                SlotData sd;
                BoneData bd;
                PathConstraintData pcd;

                int count = 0;
                string name;
                int length;
                switch (timeline)
                {
                    case ColorTimeline ct: //slots timeline
                                           //TODO: add color timeline support
                        break;

                    case AttachmentTimeline at: //slots timeline
                        sd = skeletonData.Slots.Items[at.SlotIndex];
                        name = "attachment";
                        length = at.FrameCount;

                        tl = new List<object>();
                        for (int i = 0; i < length; i++)
                        {
                            tmp = new Dictionary<string, object>();
                            tmp["time"] = at.Frames[i];
                            tmp["name"] = at.AttachmentNames[i];
                            tl.Add(tmp);
                        }
                        if (!slots.ContainsKey(sd.Name))
                            slots[sd.Name] = new Dictionary<string, List<object>>();
                        slots[sd.Name][name] = tl;
                        break;

                    case RotateTimeline rt: //bone timeline
                        name = "rotate";
                        bd = skeletonData.Bones.Items[rt.BoneIndex];
                        count = 0;
                        if (!bones.ContainsKey(bd.Name))
                            bones[bd.Name] = new Dictionary<string, List<object>>();
                        bones[bd.Name][name] = applyTimeline(rt.Frames, (f, d) =>
                        {
                            d["time"] = f.pop();
                            d["angle"] = f.pop();
                            setCurve(d, rt, count++);
                        });
                        break;

                    case TranslateTimeline tt:
                        float timelineScale = 1;
                        if (tt is ShearTimeline)
                            name = "shear";
                        else if (tt is ScaleTimeline)
                            name = "scale";
                        else
                            name = "translate";
                        bd = skeletonData.Bones.Items[tt.BoneIndex];
                        count = 0;
                        if (!bones.ContainsKey(bd.Name))
                            bones[bd.Name] = new Dictionary<string, List<object>>();
                        bones[bd.Name][name] = applyTimeline(tt.Frames, (f, d) =>
                        {
                            d["time"] = f.pop();
                            d["x"] = f.pop() / timelineScale;
                            d["y"] = f.pop() / timelineScale;
                            setCurve(d, tt, count++);
                        });
                        break;

                    case IkConstraintTimeline ikt:
                        IkConstraintData ikd = skeletonData.IkConstraints.Items[ikt.IkConstraintIndex];
                        count = 0;
                        ik[ikd.Name] = applyTimeline(ikt.Frames, (f, d) =>
                        {
                            d["time"] = f.pop();
                            d["mix"] = f.pop();
                            d["bendPositive"] = f.pop();
                            setCurve(d, ikt, count++);
                        });

                        break;

                    case TransformConstraintTimeline tft:
                        name = "transform";
                        TransformConstraintData tfd = skeletonData.TransformConstraints.Items[tft.TransformConstraintIndex];
                        count = 0;
                        if (!transform.ContainsKey(tfd.Name))
                            transform[tfd.Name] = new Dictionary<string, List<object>>();
                        transform[tfd.Name][name] = applyTimeline(tft.Frames, (f, d) =>
                        {
                            d["time"] = f.pop();
                            d["rotateMix"] = f.pop();
                            d["translateMix"] = f.pop();
                            d["scaleMix"] = f.pop();
                            d["shearMix"] = f.pop();
                            setCurve(d, tft, count++);
                        });
                        break;

                    case PathConstraintPositionTimeline pcpt:
                        name = "position";
                        if (pcpt is PathConstraintSpacingTimeline)
                            name = "spacing";
                        pcd = skeletonData.PathConstraints.Items[pcpt.PathConstraintIndex];
                        count = 0;
                        if (!paths.ContainsKey(pcd.Name))
                            paths[pcd.Name] = new Dictionary<string, List<object>>();
                        paths[pcd.Name][name] = applyTimeline(pcpt.Frames, (f, d) =>
                        {
                            d["time"] = f.pop();
                            d[name] = f.pop();
                            setCurve(d, pcpt, count++);
                        });

                        break;

                    case PathConstraintMixTimeline pcmt:
                        name = "mix";
                        //"time"], GetFloat(valueMap, "rotateMix", 1), GetFloat(valueMap, "translateMix",
                        pcd = skeletonData.PathConstraints.Items[pcmt.PathConstraintIndex];
                        count = 0;
                        if (!paths.ContainsKey(pcd.Name))
                            paths[pcd.Name] = new Dictionary<string, List<object>>();
                        paths[pcd.Name][name] = applyTimeline(pcmt.Frames, (f, d) =>
                        {
                            d["time"] = f.pop();
                            d["rotateMix"] = f.pop();
                            d["translateMix"] = f.pop();
                            setCurve(d, pcmt, count++);
                        });
                        break;

                    case DeformTimeline dt:
                        var att = dt.Attachment;

                        ////Console.WriteLine("Fuck dt");

                        //sd = skeletonData.Slots.Items[dt.SlotIndex];
                        //string skinName = dt.skin.Name;
                        //count = 0;
                        //if (!deform.ContainsKey(skinName))
                        //    deform[skinName] = new Dictionary<string, Dictionary<string, List<object>>>();
                        //if (!deform[skinName].ContainsKey(sd.Name))
                        //    deform[skinName][sd.Name] = new Dictionary<string, List<object>>();
                        //string attName = att.Name;
                        //if (attName.Contains('/'))
                        //    attName = attName.Substring(attName.LastIndexOf('/') + 1);
                        //attName = attName.Replace('-', ' ');
                        //deform[skinName][sd.Name][attName] = applyTimeline(dt.Frames, (f, d) =>
                        //{
                        //    d["time"] = f.pop();
                        //    d["offset"] = dt.offsets[count];
                        //    var verts = dt.originalVerts[count];
                        //    if (verts != null)
                        //        d["vertices"] = dt.originalVerts[count];
                        //    setCurve(d, dt, count);
                        //    count++;
                        //});

                        break;

                    case DrawOrderTimeline dot:
                        //Console.WriteLine("Fuck dot");
                        //for each frame in the draw order
                        //for (int i = 0; i < dot.FrameCount; i++)
                        //    {
                        //        //create new frame object -> order
                        //        Dictionary<string, object> order = new Dictionary<string, object>();
                        //        //each frame has a list of offset objects
                        //        List<object> list = new List<object>();
                        //        for (int k = 0; k < dot.offsetSet[i].Length; k++)
                        //        {
                        //            int offset = dot.offsetSet[i][k];
                        //            if (offset == 0)
                        //                continue;
                        //            Dictionary<string, object> temp = new Dictionary<string, object>();
                        //            sd = skeletonData.Slots.Items[k];
                        //            temp["slot"] = sd.Name;
                        //            temp["offset"] = offset;
                        //            list.Add(temp);
                        //        }
                        //        order["time"] = dot.Frames[i];
                        //        order["offsets"] = list;
                        //        drawOrder.Add(order);
                        //    }
                        break;

                    case EventTimeline et:
                        //throw new Exception("Does not support Events to JSON");
                        //Console.WriteLine("Does not support Events to JSON");
                        break;
                }
            }

            if (bones.Count != 0)
                anim["bones"] = bones;
            if (ik.Count != 0)
                anim["ik"] = ik;
            if (paths.Count != 0)
                anim["paths"] = paths;
            if (slots.Count != 0)
                anim["slots"] = slots;
            if (transform.Count != 0)
                anim["transform"] = transform;
            if (drawOrder.Count != 0)
                anim["drawOrder"] = drawOrder;
            if (events.Count != 0)
                anim["events"] = events;
            if (deform.Count != 0)
                anim["deform"] = deform;

            return anim;
        }

        private static void setCurve(Dictionary<string, object> map, CurveTimeline ct, int index)
        {
            //var curve = ct.GetCurveType(index);
            //if (curve == null) //no curve this frame
            //    return;
            //map["curve"] = curve;
            return;
        }

        private static List<Object> applyTimeline(float[] frames, Action<MyNumerator<float>, Dictionary<string, object>> action)
        {
            var ff = new MyNumerator<float>(frames);
            List<object> tl = new List<object>();
            Dictionary<string, object> tmp = new Dictionary<string, object>();
            do
            {
                tmp = new Dictionary<string, object>();
                action(ff, tmp);
                tl.Add(tmp);
            } while (ff.hasNext());
            return tl;
        }

        private class MyNumerator<T> : IEnumerator
        {
            private T[] items;
            private int index = 0;

            public MyNumerator(T[] arr)
            {
                items = arr;
            }

            public object Current => items[index];

            public T pop()
            {
                if (index >= items.Length)
                    return default(T);
                T item = items[index];
                index++;
                return item;
            }

            public bool MoveNext()
            {
                index++;
                return index >= items.Length;
            }

            public bool hasNext()
            { return index < items.Length; }

            public void Reset()
            {
                index = 0;
            }
        }
    }
}