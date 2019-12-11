using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Office.Tools.Ribbon;
using System.IO;

using Excel = Microsoft.Office.Interop.Excel;
using Office = Microsoft.Office.Core;
using Microsoft.Office.Tools.Excel;

using Rhino.Geometry;

namespace RhinoInside.Excel
{
  public partial class Ribbon1
  {
    private void Ribbon1_Load(object sender, RibbonUIEventArgs e)
    {

    }

    private void test01_click(object sender, RibbonControlEventArgs e)
    {

      //Open Excel Worksheet
      Microsoft.Office.Interop.Excel.Worksheet activeWorksheet = Globals.RhinoInsideAddIn.Application.ActiveSheet;


      try
      {
        var model = new Rhino.FileIO.File3dm();
        var sphere = new Sphere(Point3d.Origin, 12);
        var brep = sphere.ToBrep();
        var mp = new MeshingParameters(0.5);
        var mesh = Mesh.CreateFromBrep(brep, mp);
        var Vertices = mesh[0].Vertices;
        model.Objects.AddMesh(mesh[0]);


        var newFirstRow = activeWorksheet.get_Range("A1");
        int n = 1;
        newFirstRow.Offset[0, 0].Value = "index";
        newFirstRow.Offset[0, 1].Value = "X";
        newFirstRow.Offset[0, 2].Value = "Y";
        newFirstRow.Offset[0, 3].Value = "Z";
        foreach (var Vertice in mesh[0].Vertices)
        {
          newFirstRow.Offset[n, 0].Value = n;
          newFirstRow.Offset[n, 1].Value = Vertice.X;
          newFirstRow.Offset[n, 2].Value = Vertice.Y;
          newFirstRow.Offset[n, 3].Value = Vertice.Z;
          n = n + 1;
        };


        System.Windows.Forms.MessageBox.Show($"Mesh with {mesh[0].Vertices.Count} vertices created.\nMake 3dmfile in Desktop/Outputs folder.");

        var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), "Outputs", "model.3dm");
        model.Write(path, 6);

      }
      catch (Exception ex)
      {
        System.Windows.Forms.MessageBox.Show(ex.Message);
      }

    }
  }
}
