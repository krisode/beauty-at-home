using System.Collections.Generic;

namespace BeautyAtHome.ViewModels
{
    public class GalleryCM
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class GalleryVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<ImageDefaultPagingSM> Images { get; set; }

    }

    public class GallerySM
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class GalleryUM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

    }

    public class GalleryPagingSM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<ImageDefaultPagingSM> Images { get; set; }
    }

    public class ImageDefaultPagingSM
    {
        public int Id { get; set; }
        public string ImageUrl { get; set; }
        public string Description{ get; set; }
    }
}
