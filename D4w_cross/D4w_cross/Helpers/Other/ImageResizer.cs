
#if __ANDROID__
using Android.Graphics;
#endif
#if __IOS__
using UIKit;
using CoreGraphics;
using System.Drawing;
#endif
using D4w_cross.Models.API;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;


namespace D4w_cross.Helpers
{
    public static class ImageResizer
    {
        static ImageResizer()
        {
        }

        public static Image ResizeImage(Image image, float width, float height)
        {
            var res = new Image();
            var regex = new Regex("data:image.*base64,");
            var base64 = regex.Replace(image.Base64, String.Empty);
#if __IOS__
            var conv = ResizeImageIOS(Convert.FromBase64String(base64), width, height);
#endif
#if __ANDROID__
            var conv = ResizeImageAndroid(Convert.FromBase64String(base64), width, height);
#endif
            res.Base64 = Convert.ToBase64String(conv);
            return res;
        }


#if __IOS__
        public static byte[] ResizeImageIOS(byte[] imageData, float width, float height)
        {
            UIImage originalImage = ImageFromByteArray(imageData);
            UIImageOrientation orientation = originalImage.Orientation;

            //create a 24bit RGB image
            using (CGBitmapContext context = new CGBitmapContext(IntPtr.Zero,
                                                 (int)width, (int)height, 8,
                                                 (int)(4 * width), CGColorSpace.CreateDeviceRGB(),
                                                 CGImageAlphaInfo.PremultipliedFirst))
            {

                RectangleF imageRect = new RectangleF(0, 0, width, height);

                // draw the image
                context.DrawImage(imageRect, originalImage.CGImage);

                UIKit.UIImage resizedImage = UIKit.UIImage.FromImage(context.ToImage(), 0, orientation);

                // save the image as a jpeg
                return resizedImage.AsJPEG().ToArray();
            }
        }

        public static UIKit.UIImage ImageFromByteArray(byte[] data)
        {
            if (data == null)
            {
                return null;
            }

            UIKit.UIImage image;
            try
            {
                image = new UIKit.UIImage(Foundation.NSData.FromArray(data));
            }
            catch (Exception e)
            {
                Console.WriteLine("Image load failed: " + e.Message);
                return null;
            }
            return image;
        }
#endif

#if __ANDROID__

        public static byte[] ResizeImageAndroid(byte[] imageData, float width, float height)
        {
            // Load the bitmap
            Bitmap originalImage = BitmapFactory.DecodeByteArray(imageData, 0, imageData.Length);
            Bitmap resizedImage = Bitmap.CreateScaledBitmap(originalImage, (int)width, (int)height, false);

            using (MemoryStream ms = new MemoryStream())
            {
                resizedImage.Compress(Bitmap.CompressFormat.Jpeg, 100, ms);
                return ms.ToArray();
            }
        }

#endif

#if WINDOWS_PHONE
        
        public static byte[] ResizeImageWinPhone (byte[] imageData, float width, float height)
        {
            byte[] resizedData;

            using (MemoryStream streamIn = new MemoryStream (imageData))
            {
                WriteableBitmap bitmap = PictureDecoder.DecodeJpeg (streamIn, (int)width, (int)height);

                using (MemoryStream streamOut = new MemoryStream ())
                {
                    bitmap.SaveJpeg(streamOut, (int)width, (int)height, 0, 100);
                    resizedData = streamOut.ToArray();
                }
            }
            return resizedData;
        }
        
#endif


    }
}
