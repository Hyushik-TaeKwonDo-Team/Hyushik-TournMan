using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using ZXing;
using System.Drawing;
using ZXing.Common;
using Hyushik_TournMan_BLL.QrCheckin;


namespace Hyushik_TournMan_Test.Tests
{
    [TestFixture]
    public class QRTest
    {
        [Test]
        public void QrTest()
        {

            BarcodeWriter codeWriter;
            codeWriter = new BarcodeWriter
            {
                Format = (BarcodeFormat)Enum.Parse(typeof(BarcodeFormat), "QR_CODE"),
                Options = new EncodingOptions
                {
                    Height = 300,
                    Width = 300
                }
            };

            var gen = new QrGen();



            System.Drawing.Bitmap bitmap = codeWriter.Write("" + 1);

            var r = gen.getInfoFromImage(bitmap);

            Assert.AreEqual("1",r);



        }


    }
}
