using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

namespace Assets.Scripts.Application.Layout {

    public enum TargetDisplay {

        SideWindow,
        Fullscreen,
        SingleWindow
    }

    public enum LayoutType {

        Viewer3D,
        Programming,
        CommunityOutreach
    }

	[Serializable]
    public struct PictureBlock {

        public Texture Image;
		public VideoClip Video;
        public string Text;
    }

	[Serializable]
	public struct PanelBlock {

		public Texture Panel;
		public VideoClip Video;
	}

    [Serializable]
    public struct OutreachBlock
    {

        public Texture[] Slides;
        public string Header;
        public string Description;
    }
    
	[Serializable]
    public class Layout : MonoBehaviour {

        public TargetDisplay Target;
        public LayoutType LayoutType;
        public string Title;
        public PictureBlock[] Content;
		public PanelBlock[] Panels;
        public OutreachBlock[] OutreachContent;
		public bool HeaderToggle;
		public string HeaderText;
    }
}
