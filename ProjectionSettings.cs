using NetTopologySuite.Geometries;
using ProjNet.CoordinateSystems;
using ProjNet.CoordinateSystems.Transformations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForestSample
{
    public class ProjectionSettings
    {
        private static ProjectionSettings instance;

        private ProjectionSettings() { }


        public CoordinateSystem WGS84CoordinateSystem { get; private set; }
        public CoordinateSystem UTMCoordinateSystem { get; private set; }
        public CoordinateSystem WebMercatorCoordinateSystem { get; private set; }

        public MathTransform WGS84ToUTMTransformer { get; private set; }
        public MathTransform UTMToWGS84Transformer { get; private set; }

        public MathTransform WGS84ToWebMercatorTransformer { get; private set; }
        public MathTransform WebMercatorToWGS84Transformer { get; private set; }

        public GeometryFactory WGS84GeometryFactory { get; private set; }
        public GeometryFactory UTMGeometryFactory { get; private set; }
        public GeometryFactory WebMercatorGeometryFactory { get; private set; }


        public static ProjectionSettings Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ProjectionSettings();
                    instance.Init();
                }
                return instance;
            }
        }

        private void Init()
        {
            CoordinateSystem wgs84CoordinateSystem = GeographicCoordinateSystem.WGS84;
            CoordinateSystem webMercatorCoordinateSystem = ProjectedCoordinateSystem.WebMercator;


            this.WGS84CoordinateSystem = wgs84CoordinateSystem;
            this.WebMercatorCoordinateSystem = webMercatorCoordinateSystem;

            this.WGS84GeometryFactory = new GeometryFactory(new PrecisionModel(), (int)wgs84CoordinateSystem.AuthorityCode);
            this.WebMercatorGeometryFactory = new GeometryFactory(new PrecisionModel(), (int)webMercatorCoordinateSystem.AuthorityCode);

            var trfWeb = new CoordinateTransformationFactory();
            this.WGS84ToWebMercatorTransformer = trfWeb.CreateFromCoordinateSystems(wgs84CoordinateSystem, webMercatorCoordinateSystem).MathTransform;
            this.WebMercatorToWGS84Transformer = trfWeb.CreateFromCoordinateSystems(webMercatorCoordinateSystem, wgs84CoordinateSystem).MathTransform;
        }

        public static Geometry Transform(Geometry geom, MathTransform transform)
        {
            geom = geom.Copy();
            geom.Apply(new MTF(transform));
            return geom;
        }


    }

    sealed class MTF : ICoordinateSequenceFilter
    {
        private readonly MathTransform _mathTransform;

        public MTF(MathTransform mathTransform) => _mathTransform = mathTransform;

        public bool Done => false;
        public bool GeometryChanged => true;
        public void Filter(CoordinateSequence seq, int i)
        {
            double x = seq.GetX(i);
            double y = seq.GetY(i);
            double z = seq.GetZ(i);
            _mathTransform.Transform(ref x, ref y, ref z);
            seq.SetX(i, x);
            seq.SetY(i, y);
            seq.SetZ(i, z);
        }
    }
}

