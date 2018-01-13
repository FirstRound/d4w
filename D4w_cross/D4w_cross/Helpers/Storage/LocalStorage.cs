using D4w_cross.Helpers.API;
using D4w_cross.Models.API;
using D4w_cross.Models.API.Search;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace D4w_cross.Helpers
{
    class LocalStorage
    {
        private DBHelper dbHelper;
        private ImagesHelper imagesHelper;
        private CoworkingsHelper coworkingsHelper;

        public LocalStorage()
        {
            dbHelper = DBHelper.Instance;
            imagesHelper = new ImagesHelper();
            coworkingsHelper = new CoworkingsHelper();
        }
         
        public Image GetImage(int id)
        {
            var image = dbHelper.FindImage(id);
            if (image == null)
            {
                image = imagesHelper.Find(id);
                var compressed = ImageResizer.ResizeImage(image, 1024, 768);
                image.Base64 = compressed.Base64;
                dbHelper.SaveImage(image);
            }
            return image;
        }

        public ObservableCollection<Coworking> GetAllCoworkings(CoworkingSearchOptions options)
        {
            var coworkings = coworkingsHelper.GetAll(options).Coworkings;
           // foreach(var coworking in coworkings)
            //{
           //     dbHelper.SaveCoworking(coworking);
           // }
            return new ObservableCollection<Coworking>(coworkings);
        }
    }
}
