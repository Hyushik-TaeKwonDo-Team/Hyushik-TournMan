using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using ZXing;
using ZXing.Common;
using System.Drawing.Imaging;

namespace Hyushik_TournMan_BLL.QrCheckin
{
    class QrGen
    {
        BarcodeWriter codeWriter;
        private string BASE_64_HEADER = "data:image/png;base64,";

        public QrGen()
        {

            codeWriter = new BarcodeWriter
            {
                Format = (BarcodeFormat)Enum.Parse(typeof(BarcodeFormat), "QR_CODE"),
                Options = new EncodingOptions
                {
                    Height = 300,
                    Width = 300
                }
            };


        }

        public String getQrCodeFromLong(long partId)
        {

            System.Drawing.Bitmap bitmap = codeWriter.Write("" + partId);

            System.IO.MemoryStream stream = new System.IO.MemoryStream();
            bitmap.Save(stream, ImageFormat.Png);
            byte[] imageBytes = stream.ToArray();

            return BASE_64_HEADER + System.Convert.ToBase64String(imageBytes);
        }




        public Result getInfoFromImage( Bitmap img)
        {
            var hints = new Dictionary<DecodeHintType, Object>();
            var vector = new List<BarcodeFormat>
                {
		            BarcodeFormat.QR_CODE
	            };
            hints[DecodeHintType.POSSIBLE_FORMATS] = vector;
            hints[DecodeHintType.TRY_HARDER] = true;

            using (img)
            {
                LuminanceSource source;
                source = new BitmapLuminanceSource(img);
                BinaryBitmap bitmap = new BinaryBitmap(new HybridBinarizer(source));
                Result result = new MultiFormatReader().decode(bitmap, hints);
                return result;
            }
        }


    }
}
