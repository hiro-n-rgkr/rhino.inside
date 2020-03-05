using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RhinoInsideController : MonoBehaviour
{
  public GameObject _sendObject;
  public GameObject _controlPoint;

  // Start is called before the first frame update
  void Start()
  {
    string script = "!_-Grasshopper _W _T ENTER";
   
    RhinoInside.Launch();
    var ghWatcher = new GameObject("Grasshopper Geometry");
    ghWatcher.AddComponent<GrasshopperInUnity>();

    Rhino.RhinoApp.RunScript(script, false);
  }

  // Update is called once per frame
  void Update()
  {
    var pos = _controlPoint.transform.position;
    var pt = pos.ToRhino();
    using (var args = new Rhino.Runtime.NamedParametersEventArgs())
    {
      args.Set("point", new Rhino.Geometry.Point(pt));
      Rhino.Runtime.HostUtils.ExecuteNamedCallback("ControllPoint", args);
    }

    var mesh = _sendObject.GetComponent<MeshFilter>().mesh.ToRhino();
    using (var args = new Rhino.Runtime.NamedParametersEventArgs())
    {
      args.Set("mesh", mesh);
      Rhino.Runtime.HostUtils.ExecuteNamedCallback("SendObject", args);
    }
  }
}
