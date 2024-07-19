using System;
using System.Diagnostics;
using OpenTK.Graphics.OpenGL4;
using StbImageSharp;

namespace GraphicsEngine
{
    internal class Texture
    {
        public int ID { get; private set; }

        public Texture(string path)
        {
            ID = GL.GenTexture();
            LoadTexture(path);
        }

        private void LoadTexture(string path)
        {
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, ID);


            // Set texture parameters
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);

            // Load the image
            StbImage.stbi_set_flip_vertically_on_load(1);
            using var fileStream = File.OpenRead(path);
            var image = ImageResult.FromStream(fileStream, ColorComponents.RedGreenBlueAlpha);
            
            // Upload the image to the GPU
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, image.Width, image.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, image.Data);

            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
            Unbind(); 
        }

        public void Bind()
        {
            GL.BindTexture(TextureTarget.Texture2D, ID);
        }
        public void Unbind()
        {
            GL.BindTexture(TextureTarget.Texture2D, 0);
        }
        public void Delete()
        {
            GL.DeleteTexture(ID);
        }
    }
}
