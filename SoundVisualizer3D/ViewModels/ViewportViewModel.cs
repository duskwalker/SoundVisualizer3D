using System.Windows.Media;
using System.Windows.Media.Media3D;
using Prism.Mvvm;

namespace SoundVisualizer3D.ViewModels
{
    public class ViewportViewModel
        : BindableBase
    {
        private Point3DCollection _positions;
        private Int32Collection _triangleIndices;


        public Point3D Position { get; set; }
        public Vector3D LookDirection { get; set; }
        public Vector3D UpDirection { get; set; }

        public ViewportViewModel()
        {
            Position = new Point3D(-40, 40, 40);
            LookDirection = new Vector3D(40, -40, -40);
            UpDirection = new Vector3D(0, 0, 1);

            Positions = new Point3DCollection
            {
                new Point3D(0, 0, 0),
                new Point3D(10, 0, 0),
                new Point3D(10, 10, 0),
                new Point3D(0, 10, 0),
                new Point3D(0, 0, 10),
                new Point3D(10, 0, 10),
                new Point3D(10, 10, 10),
                new Point3D(0, 10, 10)
            };

            TriangleIndices = new Int32Collection
            {
                0, 1, 3, 1, 2, 3, 0, 4, 3, 4, 7, 3, 4, 6, 7, 4, 5, 6, 0, 4, 1, 1, 4, 5, 1, 2, 6, 6, 5, 1, 2, 3, 7, 7, 6, 2
            };

            //Geometry = new MeshGeometry3D
            //{
            //    Positions = new Point3DCollection
            //    {
            //        new Point3D(0,0,0),
            //        new Point3D(10,0,0),
            //        new Point3D(10,10,0),
            //        new Point3D(0,10,0),
            //        new Point3D(0,0,10),
            //        new Point3D(10,0,10),
            //        new Point3D(10,10,10),
            //        new Point3D(0,10,10)
            //    },
            //    TriangleIndices = new Int32Collection
            //    {
            //        0, 1, 3, 1, 2, 3, 0, 4, 3, 4, 7, 3, 4, 6, 7, 4, 5, 6, 0, 4, 1, 1, 4, 5, 1, 2, 6, 6, 5, 1, 2, 3, 7, 7, 6, 2
            //    }
            //};
        }

        public Int32Collection TriangleIndices
        {
            get { return _triangleIndices; }
            set { SetProperty(ref _triangleIndices, value); }
        }

        public Point3DCollection Positions
        {
            get { return _positions; }
            set { SetProperty(ref _positions, value); }
        }
    }
}