using System;
using OpenTK;
using OpenTK.Graphics;
using System.Drawing;
using System.Drawing.Imaging;
using ApexEngine.Input;

using OpenTK.Graphics.OpenGL;
using OpenTK.Audio;
using OpenTK.Audio.OpenAL;
using ApexEngine.Assets;
using ApexEngine.Audio;
using System.IO;
using ApexEngine.Math;

namespace ApexEngine.Rendering.OpenGL
{
    /// <summary>
    /// OpenTK Renderer for desktop systems.
    /// </summary>
    public class GLRenderer : Renderer
    {
        private Vector3 tmpVec1 = new Vector3(), tmpVec2 = new Vector3();
        private AudioContext audioContext;
        private bool openALSupported = true;

        public override void CreateContext(Game game, int width, int height)
        {
            using (var gameWindow = new GameWindow(width, height, new GraphicsMode(new ColorFormat(8, 8, 8, 8), 24)))
            {
                gameWindow.Title = game.Title;
                gameWindow.Load += (sender, e) => game.InitInternal();
                gameWindow.UpdateFrame += (sender, e) =>
                {
                    game.Environment.TimePerFrame = (float)e.Time;
                    game.InputManager.WINDOW_X = gameWindow.X;
                    game.InputManager.WINDOW_Y = gameWindow.Y;
                    game.InputManager.MOUSE_X = game.InputManager.WINDOW_X - OpenTK.Input.Mouse.GetCursorState().X + (game.InputManager.SCREEN_WIDTH / 2);
                    game.InputManager.MOUSE_Y = game.InputManager.WINDOW_Y - OpenTK.Input.Mouse.GetCursorState().Y + (game.InputManager.SCREEN_HEIGHT / 2);
                   //         game.Camera.Width = game.InputManager.SCREEN_WIDTH;
                    //        game.Camera.Height = game.InputManager.SCREEN_HEIGHT;
                    game.UpdateInternal();
                };
                gameWindow.RenderFrame += (sender, e) =>
                {
                    game.RenderInternal();
                    gameWindow.SwapBuffers();
                };
                gameWindow.Resize += (sender, e) =>
                {
                    GL.Viewport(0, 0, gameWindow.Width, gameWindow.Height);
                    game.InputManager.SCREEN_HEIGHT = gameWindow.Height;
                    game.InputManager.SCREEN_WIDTH = gameWindow.Width;
                };
                gameWindow.KeyDown += (sender, e) =>
                {
                    game.InputManager.KeyDown(ConvertOpenTKKey(e.Key));
                };
                gameWindow.KeyUp += (sender, e) =>
                {
                    game.InputManager.KeyUp(ConvertOpenTKKey(e.Key));
                };
                gameWindow.MouseDown += (sender, e) =>
                {
                    game.InputManager.MouseButtonDown(ConvertOpenTKMouseButton(e.Button));
                };
                gameWindow.MouseUp += (sender, e) =>
                {
                    game.InputManager.MouseButtonUp(ConvertOpenTKMouseButton(e.Button));
                };
                gameWindow.VSync = VSyncMode.On;
                gameWindow.Run(60);
            }
        }

        public override void InitAudio()
        {
            try
            {
                audioContext = new AudioContext();
            } 
            catch (Exception ex)
            {
                Console.WriteLine("OpenAL is not supported.\nDownload OpenAL from www.openal.org in order to fix this issue.");
                openALSupported = false;
            }
        }

        public override Sound LoadAudio(LoadedAsset asset)
        {
            
            Sound sound = new Sound();

            if (openALSupported)
            {

                int buffer = AL.GenBuffer();
                int source = AL.GenSource();
                int state;

                if (asset.FilePath.EndsWith("wav"))
                {
                    // Load wav
                    int channels, bits_per_sample, sample_rate;
                    byte[] wavData = LoadWave(asset.Data, out channels, out bits_per_sample, out sample_rate);

                    AL.BufferData(buffer, GetSoundFormat(channels, bits_per_sample), wavData, wavData.Length, sample_rate);
                    AL.Source(source, ALSourcei.Buffer, buffer);

                    sound.Source = source;
                    sound.Buffer = buffer;
                }

            }

            return sound;
        }

        public override AudioPlayState GetPlayState(Sound sound)
        {
            if (openALSupported)
            {
                int playing;
                AL.GetSource(sound.Source, ALGetSourcei.SourceState, out playing);
                if ((ALSourceState)playing == ALSourceState.Playing)
                    return AudioPlayState.Playing;
                else if ((ALSourceState)playing == ALSourceState.Stopped)
                    return AudioPlayState.Stopped;
                else if ((ALSourceState)playing == ALSourceState.Paused)
                    return AudioPlayState.Paused;
            }
            return AudioPlayState.Stopped;
        }

        public override void PlaySound(Sound sound)
        {
            if (openALSupported)
            {
                int playing;
                AL.GetSource(sound.Source, ALGetSourcei.SourceState, out playing);
                if ((ALSourceState)playing != ALSourceState.Playing)
                {
                    AL.SourcePlay(sound.Source);
                }
            }
        }

        public override void StopSound(Sound sound)
        {
            if (openALSupported)
            {
                int playing;
                AL.GetSource(sound.Source, ALGetSourcei.SourceState, out playing);
                if ((ALSourceState)playing != ALSourceState.Stopped)
                {
                    AL.SourceStop(sound.Source);
                }
            }
        }

        public override void PauseSound(Sound sound)
        {
            if (openALSupported)
            {
                int playing;
                AL.GetSource(sound.Source, ALGetSourcei.SourceState, out playing);
                if ((ALSourceState)playing != ALSourceState.Paused)
                {
                    AL.SourcePause(sound.Source);
                }
            }
        }

        public override void SetAudioValues(Sound sound, float pitch, float gain, Vector3f position, Vector3f velocity)
        {
            if (openALSupported)
            {
                AL.Source(sound.Source, ALSourcef.Gain, gain);
                AL.Source(sound.Source, ALSourcef.Pitch, pitch);
                AL.Source(sound.Source, ALSource3f.Position, position.x, position.y, position.z);
                AL.Source(sound.Source, ALSource3f.Velocity, velocity.x, velocity.y, velocity.z);
            }
        }

        public override void SetAudioListenerValues(Camera cam)
        {
            if (openALSupported)
            {
                tmpVec1.X = cam.Direction.x;
                tmpVec1.Y = cam.Direction.y;
                tmpVec1.Z = cam.Direction.z;

                tmpVec2.X = cam.Up.x;
                tmpVec2.Y = cam.Up.y;
                tmpVec2.Z = cam.Up.z;

                AL.Listener(ALListener3f.Position, cam.Translation.x, cam.Translation.y, cam.Translation.z);
                AL.Listener(ALListener3f.Velocity, 0, 0, 0);
                AL.Listener(ALListenerfv.Orientation, ref tmpVec1, ref tmpVec2);
            }
        }

        private static ALFormat GetSoundFormat(int channels, int bits)
        {
            switch (channels)
            {
                case 1: return bits == 8 ? ALFormat.Mono8 : ALFormat.Mono16;
                case 2: return bits == 8 ? ALFormat.Stereo8 : ALFormat.Stereo16;
                default: throw new NotSupportedException("The specified sound format is not supported.");
            }
        }

        // Loads a wave/riff audio file.
        private static byte[] LoadWave(Stream stream, out int channels, out int bits, out int rate)
        {
            if (stream == null)
                throw new ArgumentNullException("stream");

            using (BinaryReader reader = new BinaryReader(stream))
            {
                // RIFF header
                string signature = new string(reader.ReadChars(4));
                if (signature != "RIFF")
                    throw new NotSupportedException("Specified stream is not a wave file.");

                int riff_chunck_size = reader.ReadInt32();

                string format = new string(reader.ReadChars(4));
                if (format != "WAVE")
                    throw new NotSupportedException("Specified stream is not a wave file.");

                // WAVE header
                string format_signature = new string(reader.ReadChars(4));
                if (format_signature != "fmt ")
                    throw new NotSupportedException("Specified wave file is not supported.");

                int format_chunk_size = reader.ReadInt32();
                int audio_format = reader.ReadInt16();
                int num_channels = reader.ReadInt16();
                int sample_rate = reader.ReadInt32();
                int byte_rate = reader.ReadInt32();
                int block_align = reader.ReadInt16();
                int bits_per_sample = reader.ReadInt16();

                string data_signature = new string(reader.ReadChars(4));
                if (data_signature != "data")
                    throw new NotSupportedException("Specified wave file is not supported.");

                int data_chunk_size = reader.ReadInt32();

                channels = num_channels;
                bits = bits_per_sample;
                rate = sample_rate;

                return reader.ReadBytes((int)reader.BaseStream.Length);
            }
        }

        #region Util

        private static InputManager.MouseButton ConvertOpenTKMouseButton(OpenTK.Input.MouseButton btn)
        {
            if (btn == OpenTK.Input.MouseButton.Left)
                return InputManager.MouseButton.Left;
            else if (btn == OpenTK.Input.MouseButton.Middle)
                return InputManager.MouseButton.Middle;
            else if (btn == OpenTK.Input.MouseButton.Right)
                return InputManager.MouseButton.Right;
            else
                return InputManager.MouseButton.None;
        }

        private static InputManager.KeyboardKey ConvertOpenTKKey(OpenTK.Input.Key key)
        {
            if (key == OpenTK.Input.Key.A)
                return InputManager.KeyboardKey.A;
            else if (key == OpenTK.Input.Key.B)
                return InputManager.KeyboardKey.B;
            else if (key == OpenTK.Input.Key.C)
                return InputManager.KeyboardKey.C;
            else if (key == OpenTK.Input.Key.D)
                return InputManager.KeyboardKey.D;
            else if (key == OpenTK.Input.Key.E)
                return InputManager.KeyboardKey.E;
            else if (key == OpenTK.Input.Key.F)
                return InputManager.KeyboardKey.F;
            else if (key == OpenTK.Input.Key.G)
                return InputManager.KeyboardKey.G;
            else if (key == OpenTK.Input.Key.H)
                return InputManager.KeyboardKey.H;
            else if (key == OpenTK.Input.Key.I)
                return InputManager.KeyboardKey.I;
            else if (key == OpenTK.Input.Key.J)
                return InputManager.KeyboardKey.J;
            else if (key == OpenTK.Input.Key.K)
                return InputManager.KeyboardKey.K;
            else if (key == OpenTK.Input.Key.L)
                return InputManager.KeyboardKey.L;
            else if (key == OpenTK.Input.Key.M)
                return InputManager.KeyboardKey.M;
            else if (key == OpenTK.Input.Key.N)
                return InputManager.KeyboardKey.N;
            else if (key == OpenTK.Input.Key.O)
                return InputManager.KeyboardKey.O;
            else if (key == OpenTK.Input.Key.P)
                return InputManager.KeyboardKey.P;
            else if (key == OpenTK.Input.Key.Q)
                return InputManager.KeyboardKey.Q;
            else if (key == OpenTK.Input.Key.R)
                return InputManager.KeyboardKey.R;
            else if (key == OpenTK.Input.Key.S)
                return InputManager.KeyboardKey.S;
            else if (key == OpenTK.Input.Key.T)
                return InputManager.KeyboardKey.T;
            else if (key == OpenTK.Input.Key.U)
                return InputManager.KeyboardKey.U;
            else if (key == OpenTK.Input.Key.V)
                return InputManager.KeyboardKey.V;
            else if (key == OpenTK.Input.Key.W)
                return InputManager.KeyboardKey.W;
            else if (key == OpenTK.Input.Key.X)
                return InputManager.KeyboardKey.X;
            else if (key == OpenTK.Input.Key.Y)
                return InputManager.KeyboardKey.Y;
            else if (key == OpenTK.Input.Key.Z)
                return InputManager.KeyboardKey.Z;
            else if (key == OpenTK.Input.Key.Space)
                return InputManager.KeyboardKey.Space;
            else if (key == OpenTK.Input.Key.BackSpace)
                return InputManager.KeyboardKey.Backspace;
            else if (key == OpenTK.Input.Key.AltLeft)
                return InputManager.KeyboardKey.LeftAlt;
            else if (key == OpenTK.Input.Key.AltRight)
                return InputManager.KeyboardKey.RightAlt;
            else if (key == OpenTK.Input.Key.ControlLeft)
                return InputManager.KeyboardKey.LeftCtrl;
            else if (key == OpenTK.Input.Key.ControlRight)
                return InputManager.KeyboardKey.RightCtrl;
            else if (key == OpenTK.Input.Key.RShift)
                return InputManager.KeyboardKey.RightShift;
            else if (key == OpenTK.Input.Key.LShift)
                return InputManager.KeyboardKey.LeftShift;
            else if (key == OpenTK.Input.Key.Number0)
                return InputManager.KeyboardKey.Num0;
            else if (key == OpenTK.Input.Key.Number1)
                return InputManager.KeyboardKey.Num1;
            else if (key == OpenTK.Input.Key.Number2)
                return InputManager.KeyboardKey.Num2;
            else if (key == OpenTK.Input.Key.Number3)
                return InputManager.KeyboardKey.Num3;
            else if (key == OpenTK.Input.Key.Number4)
                return InputManager.KeyboardKey.Num4;
            else if (key == OpenTK.Input.Key.Number5)
                return InputManager.KeyboardKey.Num5;
            else if (key == OpenTK.Input.Key.Number6)
                return InputManager.KeyboardKey.Num6;
            else if (key == OpenTK.Input.Key.Number7)
                return InputManager.KeyboardKey.Num7;
            else if (key == OpenTK.Input.Key.Number8)
                return InputManager.KeyboardKey.Num8;
            else if (key == OpenTK.Input.Key.Number9)
                return InputManager.KeyboardKey.Num9;
            else
                return InputManager.KeyboardKey.None;
        }

        #endregion

        #region Mesh stuff

        public override void CreateMesh(Mesh mesh)
        {
            GL.GenBuffers(1, out mesh.vbo);
            GL.GenBuffers(1, out mesh.ibo);
        }

        public override void UploadMesh(Mesh mesh)
        {
            mesh.size = mesh.indices.Count;
            float[] vertexBuffer = Util.MeshUtil.CreateFloatBuffer(mesh);
            int[] indexBuffer = new int[mesh.indices.Count];
            for (int i = 0; i < mesh.indices.Count; i++)
            {
                indexBuffer[i] = mesh.indices[i];
            }
            GL.BindBuffer(BufferTarget.ArrayBuffer, mesh.vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(vertexBuffer.Length * sizeof(float)), vertexBuffer, BufferUsageHint.StaticDraw);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, mesh.ibo);
            GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)(indexBuffer.Length * sizeof(int)), indexBuffer, BufferUsageHint.StaticDraw);
        }

        public override void DeleteMesh(Mesh mesh)
        {
            GL.DeleteBuffers(1, ref mesh.vbo);
            GL.DeleteBuffers(1, ref mesh.ibo);
        }

        public override void RenderMesh(Mesh mesh)
        {
            BeginMode primitiveType = BeginMode.Triangles;
            if (mesh.PrimitiveType == Mesh.PrimitiveTypes.Triangles)
                primitiveType = BeginMode.Triangles;
            else if (mesh.PrimitiveType == Mesh.PrimitiveTypes.TriangleStrip)
                primitiveType = BeginMode.TriangleStrip;
            else if (mesh.PrimitiveType == Mesh.PrimitiveTypes.TriangleFan)
                primitiveType = BeginMode.TriangleFan;
            else if (mesh.PrimitiveType == Mesh.PrimitiveTypes.Quads)
                primitiveType = BeginMode.Quads;
            else if (mesh.PrimitiveType == Mesh.PrimitiveTypes.Quads)
                primitiveType = BeginMode.QuadStrip;
            else if (mesh.PrimitiveType == Mesh.PrimitiveTypes.Patches)
                primitiveType = BeginMode.Patches;
            else if (mesh.PrimitiveType == Mesh.PrimitiveTypes.Points)
                primitiveType = BeginMode.Points;
            else if (mesh.PrimitiveType == Mesh.PrimitiveTypes.Lines)
                primitiveType = BeginMode.Lines;

            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);

            GL.BindBuffer(BufferTarget.ArrayBuffer, mesh.vbo);
            if (mesh.GetAttributes().HasAttribute(VertexAttributes.POSITIONS))
            {
                GL.EnableVertexAttribArray(0);
                GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, mesh.vertexSize * sizeof(float), mesh.GetAttributes().GetPositionOffset());
            }
            if (mesh.GetAttributes().HasAttribute(VertexAttributes.TEXCOORDS0))
            {
                GL.EnableVertexAttribArray(1);
                GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, mesh.vertexSize * sizeof(float), mesh.GetAttributes().GetTexCoord0Offset());
            }
            if (mesh.GetAttributes().HasAttribute(VertexAttributes.TEXCOORDS1))
            {
                GL.EnableVertexAttribArray(2);
                GL.VertexAttribPointer(2, 2, VertexAttribPointerType.Float, false, mesh.vertexSize * sizeof(float), mesh.GetAttributes().GetTexCoord1Offset());
            }
            if (mesh.GetAttributes().HasAttribute(VertexAttributes.NORMALS))
            {
                GL.EnableVertexAttribArray(3);
                GL.VertexAttribPointer(3, 3, VertexAttribPointerType.Float, false, mesh.vertexSize * sizeof(float), mesh.GetAttributes().GetNormalOffset());
            }
            if (mesh.GetAttributes().HasAttribute(VertexAttributes.TANGENTS))
            {
                GL.EnableVertexAttribArray(4);
                GL.VertexAttribPointer(4, 3, VertexAttribPointerType.Float, false, mesh.vertexSize * sizeof(float), mesh.GetAttributes().GetTangentOffset());
            }
            if (mesh.GetAttributes().HasAttribute(VertexAttributes.BITANGENTS))
            {
                GL.EnableVertexAttribArray(5);
                GL.VertexAttribPointer(5, 3, VertexAttribPointerType.Float, false, mesh.vertexSize * sizeof(float), mesh.GetAttributes().GetBitangentOffset());
            }
            if (mesh.GetAttributes().HasAttribute(VertexAttributes.BONEWEIGHTS))
            {
                GL.EnableVertexAttribArray(6);
                GL.VertexAttribPointer(6, 4, VertexAttribPointerType.Float, false, mesh.vertexSize * sizeof(float), mesh.GetAttributes().GetBoneWeightOffset());
            }
            if (mesh.GetAttributes().HasAttribute(VertexAttributes.BONEINDICES))
            {
                GL.EnableVertexAttribArray(7);
                GL.VertexAttribPointer(7, 4, VertexAttribPointerType.Float, false, mesh.vertexSize * sizeof(float), mesh.GetAttributes().GetBoneIndexOffset());
            }

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, mesh.ibo);
            GL.DrawElements(primitiveType, mesh.size, DrawElementsType.UnsignedInt, 0);

            if (mesh.GetAttributes().HasAttribute(VertexAttributes.POSITIONS)) GL.DisableVertexAttribArray(0);
            if (mesh.GetAttributes().HasAttribute(VertexAttributes.TEXCOORDS0)) GL.DisableVertexAttribArray(1);
            if (mesh.GetAttributes().HasAttribute(VertexAttributes.TEXCOORDS1)) GL.DisableVertexAttribArray(2);
            if (mesh.GetAttributes().HasAttribute(VertexAttributes.NORMALS)) GL.DisableVertexAttribArray(3);
            if (mesh.GetAttributes().HasAttribute(VertexAttributes.TANGENTS)) GL.DisableVertexAttribArray(4);
            if (mesh.GetAttributes().HasAttribute(VertexAttributes.BITANGENTS)) GL.DisableVertexAttribArray(5);
            if (mesh.GetAttributes().HasAttribute(VertexAttributes.BONEWEIGHTS)) GL.DisableVertexAttribArray(6);
            if (mesh.GetAttributes().HasAttribute(VertexAttributes.BONEINDICES)) GL.DisableVertexAttribArray(7);
        }

        #endregion Mesh stuff

        #region Texture stuff

        public override Texture2D LoadTexture2D(LoadedAsset asset)
        {
            Bitmap bmp = null;
            if (asset.FilePath.EndsWith(".tga"))
            {
                bmp = ApexEngine.Assets.Util.TargaImage.LoadTargaImage(asset.Data);
            }
            else
                bmp = new Bitmap(asset.Data);
            BitmapData bmp_data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            Texture2D tex = new Texture2D(Texture.GenTextureID());
            tex.TexturePath = asset.FilePath;
            tex.Use();
            tex.SetWrap(Convert.ToInt32(TextureWrapMode.Repeat), Convert.ToInt32(TextureWrapMode.Repeat));
            tex.SetFilter((int)TextureMinFilter.LinearMipmapLinear, (int)TextureMagFilter.Linear);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba8, bmp_data.Width, bmp_data.Height, 0,
                OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, bmp_data.Scan0);
            tex.GenerateMipmap();
            tex.Width = bmp.Width;
            tex.Height = bmp.Height;

            bmp.UnlockBits(bmp_data);
            Texture2D.Clear();
            return tex;
        }

        public override void TextureWrap2D(Texture.WrapMode s, Texture.WrapMode t)
        {
            TextureWrapMode tkWrapModeS = TextureWrapMode.Repeat, tkWrapModeT = TextureWrapMode.Repeat;

            if (s == Texture.WrapMode.Repeat)
                tkWrapModeS = TextureWrapMode.Repeat;
            else if (s == Texture.WrapMode.ClampToBorder)
                tkWrapModeS = TextureWrapMode.ClampToBorder;
            else if (s == Texture.WrapMode.ClampToEdge)
                tkWrapModeS = TextureWrapMode.ClampToEdge;

            if (t == Texture.WrapMode.Repeat)
                tkWrapModeT = TextureWrapMode.Repeat;
            else if (t == Texture.WrapMode.ClampToBorder)
                tkWrapModeT = TextureWrapMode.ClampToBorder;
            else if (t == Texture.WrapMode.ClampToEdge)
                tkWrapModeT = TextureWrapMode.ClampToEdge;

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, Convert.ToInt32(tkWrapModeS));
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, Convert.ToInt32(tkWrapModeT));
        }

        public override void TextureWrapCube(Texture.WrapMode r, Texture.WrapMode s, Texture.WrapMode t)
        {
            TextureWrapMode tkWrapModeR = TextureWrapMode.Repeat, tkWrapModeS = TextureWrapMode.Repeat, tkWrapModeT = TextureWrapMode.Repeat;

            if (r == Texture.WrapMode.Repeat)
                tkWrapModeR = TextureWrapMode.Repeat;
            else if (r == Texture.WrapMode.ClampToBorder)
                tkWrapModeR = TextureWrapMode.ClampToBorder;
            else if (r == Texture.WrapMode.ClampToEdge)
                tkWrapModeR = TextureWrapMode.ClampToEdge;

            if (s == Texture.WrapMode.Repeat)
                tkWrapModeS = TextureWrapMode.Repeat;
            else if (s == Texture.WrapMode.ClampToBorder)
                tkWrapModeS = TextureWrapMode.ClampToBorder;
            else if (s == Texture.WrapMode.ClampToEdge)
                tkWrapModeS = TextureWrapMode.ClampToEdge;

            if (t == Texture.WrapMode.Repeat)
                tkWrapModeT = TextureWrapMode.Repeat;
            else if (t == Texture.WrapMode.ClampToBorder)
                tkWrapModeT = TextureWrapMode.ClampToBorder;
            else if (t == Texture.WrapMode.ClampToEdge)
                tkWrapModeT = TextureWrapMode.ClampToEdge;

            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapR, Convert.ToInt32(tkWrapModeR));
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapS, Convert.ToInt32(tkWrapModeS));
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapT, Convert.ToInt32(tkWrapModeT));
        }

        private TextureTarget[] cubeMapTargets = new TextureTarget[] {TextureTarget.TextureCubeMapPositiveX, TextureTarget.TextureCubeMapNegativeX,
                                                            TextureTarget.TextureCubeMapPositiveY, TextureTarget.TextureCubeMapNegativeY,
                                                            TextureTarget.TextureCubeMapPositiveZ, TextureTarget.TextureCubeMapNegativeZ};

        public override Cubemap LoadCubemap(string[] filepaths)
        {
            if (filepaths.Length != 6)
                throw new Exception("A cubemap is made up of exactly six textures. " + filepaths.Length + " textures were supplied.");
            GL.Enable(EnableCap.TextureCubeMap);
            GL.Enable(EnableCap.TextureCubeMapSeamless);
            int id = Texture.GenTextureID();
            Cubemap res = new Cubemap(id);
            res.Use();
            for (int i = 0; i < filepaths.Length; i++)
            {
                Bitmap bmp = new Bitmap(filepaths[i]);
                BitmapData bmp_data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

                GL.TexImage2D(cubeMapTargets[i], 0, PixelInternalFormat.Rgba, bmp_data.Width, bmp_data.Height, 0,
                    OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, bmp_data.Scan0);

                bmp.UnlockBits(bmp_data);
            }
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapR, (int)TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.LinearMipmapLinear);
            res.GenerateMipmap();
            GL.BindTexture(TextureTarget.TextureCubeMap, 0);
            return res;
        }

        public override void GenerateMipmap2D()
        {
            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
        }

        public override void GenerateMipmapCubemap()
        {
            GL.GenerateMipmap(GenerateMipmapTarget.TextureCubeMap);
        }

        public override void TextureFilter2D(int min, int mag)
        {
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, min);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, mag);
        }

        public override int GenTexture()
        {
            return GL.GenTexture();
        }

        public override void GenTextures(int n, out int textures)
        {
            GL.GenTextures(n, out textures);
        }

        public override void BindTexture2D(int i)
        {
            GL.BindTexture(TextureTarget.Texture2D, i);
        }

        public override void BindTexture3D(int i)
        {
            GL.BindTexture(TextureTarget.Texture3D, i);
        }

        public override void BindTextureCubemap(int i)
        {
            GL.BindTexture(TextureTarget.TextureCubeMap, i);
        }

        public override void ActiveTextureSlot(int slot)
        {
            GL.ActiveTexture(TextureUnit.Texture0 + slot);
        }

        public override void CopyScreenToTexture2D(int width, int height)
        {
            GL.CopyTexSubImage2D(TextureTarget.Texture2D, 0, 0, 0, 0, 0, width, height);
        }

        #endregion Texture stuff

        #region Shader stuff

        public override int GenerateShaderProgram()
        {
            int id;
            id = GL.CreateProgram();
            if (id == 0)
            {
                throw new Exception("An error occurred while creating the shader!");
            }
            return id;
        }

        public override void BindShaderProgram(int id)
        {
            GL.UseProgram(id);
        }

        public override void SetShaderUniform(int id, string name, int i)
        {
            int loc = GL.GetUniformLocation(id, name);
            GL.Uniform1(loc, i);
        }

        public override void CompileShaderProgram(int id)
        {
            GL.BindAttribLocation(id, 0, Shader.A_POSITION);
            GL.BindAttribLocation(id, 1, Shader.A_TEXCOORD0);
            GL.BindAttribLocation(id, 2, Shader.A_TEXCOORD1);
            GL.BindAttribLocation(id, 3, Shader.A_NORMAL);
            GL.BindAttribLocation(id, 4, Shader.A_TANGENT);
            GL.BindAttribLocation(id, 5, Shader.A_BITANGENT);
            GL.BindAttribLocation(id, 6, Shader.A_BONEWEIGHT);
            GL.BindAttribLocation(id, 7, Shader.A_BONEINDEX);
            GL.LinkProgram(id);
            GL.ValidateProgram(id);
        }

        public override void AddShader(int id, string code, Shader.ShaderTypes type)
        {
            ShaderType stype = ShaderType.VertexShader;

            if (type == Shader.ShaderTypes.Vertex)
                stype = ShaderType.VertexShader;
            else if (type == Shader.ShaderTypes.Fragment)
                stype = ShaderType.FragmentShader;
            else if (type == Shader.ShaderTypes.Geometry)
                stype = ShaderType.GeometryShader;
            else if (type == Shader.ShaderTypes.TessControl)
                stype = ShaderType.TessControlShader;
            else if (type == Shader.ShaderTypes.TessEval)
                stype = ShaderType.TessEvaluationShader;

            int shader = GL.CreateShader(stype);
            if (shader == 0)
            {
                throw new Exception("Error creating shader.\n\tShader type: " + type + "\n\tCode: " + code);
            }
            GL.ShaderSource(shader, code);
            GL.CompileShader(shader);
            GL.AttachShader(id, shader);
            int status = -1;
            string info = "";
            GL.GetShaderInfoLog(shader, out info);
            GL.GetShader(shader, ShaderParameter.CompileStatus, out status);
            if (status != 1)
            {
                Console.WriteLine("Shader compiler error!\nType: " + type.ToString() + "\nName: " + this.GetType().ToString() + "\n\n"/* + "Source code:\n" + code + "\n\n"*/ +
                           info + "\n" + "Status Code: " + status.ToString());
            }
        }

        public override void SetShaderUniform(int id, string name, float i)
        {
            int loc = GL.GetUniformLocation(id, name);
            GL.Uniform1(loc, i);
        }

        public override void SetShaderUniform(int id, string name, float x, float y)
        {
            int loc = GL.GetUniformLocation(id, name);
            GL.Uniform2(loc, x, y);
        }

        public override void SetShaderUniform(int id, string name, float x, float y, float z)
        {
            int loc = GL.GetUniformLocation(id, name);
            GL.Uniform3(loc, x, y, z);
        }

        public override void SetShaderUniform(int id, string name, float x, float y, float z, float w)
        {
            int loc = GL.GetUniformLocation(id, name);
            GL.Uniform4(loc, x, y, z, w);
        }

        public override void SetShaderUniform(int id, string name, float[] matrix)
        {
            int loc = GL.GetUniformLocation(id, name);
            GL.UniformMatrix4(loc, 1, true, matrix);
        }

        #endregion Shader stuff

        #region Framebuffer stuff

        public override void GenFramebuffers(int n, out int framebuffers)
        {
            GL.GenFramebuffers(n, out framebuffers);
        }

        public override void SetupFramebuffer(int framebufferID, int colorTextureID, int depthTextureID, int width, int height)
        {
            RenderManager.Renderer.BindTexture2D(colorTextureID);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba16, width, height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Rgba, PixelType.Int, IntPtr.Zero);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);
            GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, TextureTarget.Texture2D, colorTextureID, 0);

            GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, depthTextureID);
            RenderManager.Renderer.BindTexture2D(depthTextureID);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.DepthComponent32f, width, height, 0, OpenTK.Graphics.OpenGL.PixelFormat.DepthComponent, PixelType.Int, IntPtr.Zero);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);
            GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthAttachment, TextureTarget.Texture2D, depthTextureID, 0);
        }

        public override void BindFramebuffer(int id)
        {
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, id);
        }

        #endregion Framebuffer stuff

        #region Rendering stuff

        public override void Viewport(int x, int y, int width, int height)
        {
            GL.Viewport(x, y, width, height);
        }

        public override void Clear(bool clearColor, bool clearDepth, bool clearStencil)
        {
            // kind of tedious... but it works
            if (clearColor && clearDepth && clearStencil)
                GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit | ClearBufferMask.StencilBufferBit);
            else if (clearColor && clearDepth)
                GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            else if (clearStencil && clearDepth)
                GL.Clear(ClearBufferMask.StencilBufferBit | ClearBufferMask.DepthBufferBit);
            else if (clearColor && clearStencil)
                GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.StencilBufferBit);
            else if (clearColor)
                GL.Clear(ClearBufferMask.ColorBufferBit);
            else if (clearDepth)
                GL.Clear(ClearBufferMask.DepthBufferBit);
            else if (clearStencil)
                GL.Clear(ClearBufferMask.StencilBufferBit);
        }

        public override void ClearColor(float r, float g, float b, float a)
        {
            GL.ClearColor(r, g, b, a);
        }

        public override void DrawVertex(float x, float y)
        {
            GL.Vertex2(x, y);
        }

        public override void DrawVertex(float x, float y, float z)
        {
            GL.Vertex3(x, y, z);
        }

        #endregion

        #region Enable/disable stuff

        public override void SetDepthTest(bool depthTest)
        {
            if (depthTest)
                GL.Enable(EnableCap.DepthTest);
            else
                GL.Disable(EnableCap.DepthTest);
        }

        public override void SetDepthMask(bool depthMask)
        {
            GL.DepthMask(depthMask);
        }

        public override void SetDepthClamp(bool depthClamp)
        {
            if (depthClamp)
                GL.Enable(EnableCap.DepthClamp);
            else
                GL.Disable(EnableCap.DepthClamp);
        }

        public override void SetBlend(bool blend)
        {
            if (blend)
                GL.Enable(EnableCap.Blend);
            else
                GL.Disable(EnableCap.Blend);
        }

        public override void SetBlendMode(BlendMode blendMode)
        {
            if (blendMode == BlendMode.AlphaBlend)
            {
                GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
            }
        }

        public override void SetCullFace(bool cullFace)
        {
            if (cullFace)
                GL.Enable(EnableCap.CullFace);
            else
                GL.Disable(EnableCap.CullFace);
        }

        public override void SetFaceToCull(Face face)
        {
            if (face == Face.Back)
                GL.CullFace(CullFaceMode.Back);
            else if (face == Face.Front)
                GL.CullFace(CullFaceMode.Front);
        }

        public override void SetFaceDirection(FaceDirection faceDirection)
        {
            if (faceDirection == FaceDirection.Ccw)
                GL.FrontFace(FrontFaceDirection.Ccw);
            else if (faceDirection == FaceDirection.Cw)
                GL.FrontFace(FrontFaceDirection.Cw);
        }

        #endregion
    }
}