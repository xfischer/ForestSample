using NetTopologySuite.Features;
using NetTopologySuite.Geometries;
using NetTopologySuite.Geometries.Utilities;
using NetTopologySuite.IO;
using ProjNet.CoordinateSystems;
using ProjNet.CoordinateSystems.Transformations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForestSample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Useful visualization/editing tools :
                // WKT: http://arthur-e.github.io/Wicket/sandbox-gmaps3.html
                // GeoJSON : http://geojson.io/


                // Read GeoJSON
                const string json = "{\"type\": \"Polygon\",\"coordinates\": [[[-111.70898437499999,38.71980474264237],[-111.3134765625,37.54457732085582],[-109.77539062499999,37.89219554724437],[-110.390625,39.774769485295465],[-111.70898437499999,38.71980474264237]]]}}";                
                Geometry result = new GeoJsonReader().Read<Geometry>(json);
                Console.WriteLine("Read geometry: " + result.ToString());

                // Compute envelope
                Geometry envelopeFromDB = result.Envelope;
                Console.WriteLine("Envelope (Lat/Lon): " + envelopeFromDB.ToString());

                // Build a polygon from scratch
                Polygon polygon = new Polygon(new LinearRing(new [] { 
                    new Coordinate(-114, 39.5),
                    new Coordinate(-114, 36.25),
                    new Coordinate(-110, 36.25),
                    new Coordinate(-110, 39.5),
                    new Coordinate(-114, 39.5) }));
                Console.WriteLine("Manual Envelope (Lat/Lon): " + polygon.ToString());


                // Intersection test
                bool intersects = envelopeFromDB.Intersects(polygon);
                Console.WriteLine("Intersection result: " + polygon.ToString());

                // Reprojection functions. May help
                //
                //var envelope3857 = ProjectionSettings.Transform(envelopeFromDB, ProjectionSettings.Instance.WGS84ToWebMercatorTransformer);
                //Console.WriteLine("Envelope (WebMercator) : " + envelope3857.ToString());
                //var envelope4326 = ProjectionSettings.Transform(envelope3857, ProjectionSettings.Instance.WebMercatorToWGS84Transformer);
                //Console.WriteLine("Envelope (Back to Lat/Lon) : " + envelope4326.ToString());

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            finally
            {
                Console.ReadLine();
            }

        }

      

       
    }
}
