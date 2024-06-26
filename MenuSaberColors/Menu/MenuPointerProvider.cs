using MenuSaberColors.Models;
using System.Linq;
using UnityEngine;
using Zenject;

namespace MenuSaberColors.Menu
{
    internal class MenuPointerProvider : IInitializable
    {
        public MenuPointer LeftPointer { get; private set; }

        public MenuPointer RightPointer { get; private set; }

        public void Initialize()
        {
            var controllers = Resources.FindObjectsOfTypeAll<VRController>();
            var left = controllers.First(c => c.gameObject.name == "ControllerLeft");
            var right = controllers.First(c => c.gameObject.name == "ControllerRight");
            LeftPointer = new(left.GetComponentsInChildren<SetSaberGlowColor>(true));
            RightPointer = new(right.GetComponentsInChildren<SetSaberGlowColor>(true));
        }
    }
}
