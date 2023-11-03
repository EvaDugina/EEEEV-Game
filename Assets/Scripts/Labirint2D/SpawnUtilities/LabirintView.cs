using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


public class LabirintView : MonoBehaviour
{

    [Header("Материал пола в зависимости от типа клетки")]
    public Material Field;
    public Material Room;
    public Material Corridor;

    public Material Default;
    public Material Portal;
    public Material Start;
    public Material Finish;


}


