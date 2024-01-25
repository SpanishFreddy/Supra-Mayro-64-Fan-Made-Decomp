using System;
using System.IO;
using UnityEngine;

namespace UTJ.FrameCapturer
{
	[Serializable]
	public class DataPath
	{
		public enum Root
		{
			Absolute = 0,
			Current = 1,
			PersistentData = 2,
			StreamingAssets = 3,
			TemporaryCache = 4,
			DataPath = 5
		}

		[SerializeField]
		private Root m_root = Root.Current;

		[SerializeField]
		private string m_leaf = string.Empty;

		public Root root
		{
			get
			{
				return m_root;
			}
			set
			{
				m_root = value;
			}
		}

		public string leaf
		{
			get
			{
				return m_leaf;
			}
			set
			{
				m_leaf = value;
			}
		}

		public bool readOnly
		{
			get
			{
				return false;
			}
			set
			{
			}
		}

		public DataPath()
		{
		}

		public DataPath(Root root, string leaf)
		{
			m_root = root;
			m_leaf = leaf;
		}

		public DataPath(string path)
		{
			if (path.Contains(Application.streamingAssetsPath))
			{
				m_root = Root.StreamingAssets;
				m_leaf = path.Replace(Application.streamingAssetsPath, string.Empty).TrimStart('/');
				return;
			}
			if (path.Contains(Application.dataPath))
			{
				m_root = Root.DataPath;
				m_leaf = path.Replace(Application.dataPath, string.Empty).TrimStart('/');
				return;
			}
			if (path.Contains(Application.persistentDataPath))
			{
				m_root = Root.PersistentData;
				m_leaf = path.Replace(Application.persistentDataPath, string.Empty).TrimStart('/');
				return;
			}
			if (path.Contains(Application.temporaryCachePath))
			{
				m_root = Root.TemporaryCache;
				m_leaf = path.Replace(Application.temporaryCachePath, string.Empty).TrimStart('/');
				return;
			}
			string text = Directory.GetCurrentDirectory().Replace("\\", "/");
			if (path.Contains(text))
			{
				m_root = Root.Current;
				m_leaf = path.Replace(text, string.Empty).TrimStart('/');
			}
			else
			{
				m_root = Root.Absolute;
				m_leaf = path;
			}
		}

		public string GetFullPath()
		{
			if (m_root == Root.Absolute)
			{
				return m_leaf;
			}
			if (m_root == Root.Current)
			{
				return (m_leaf.Length != 0) ? ("./" + m_leaf) : ".";
			}
			string text = string.Empty;
			switch (m_root)
			{
			case Root.PersistentData:
				text = Application.persistentDataPath;
				break;
			case Root.StreamingAssets:
				text = Application.streamingAssetsPath;
				break;
			case Root.TemporaryCache:
				text = Application.temporaryCachePath;
				break;
			case Root.DataPath:
				text = Application.dataPath;
				break;
			}
			if (!m_leaf.StartsWith("/"))
			{
				text += "/";
			}
			return text + m_leaf;
		}

		public void CreateDirectory()
		{
			try
			{
				string fullPath = GetFullPath();
				if (fullPath.Length > 0)
				{
					Directory.CreateDirectory(fullPath);
				}
			}
			catch (Exception)
			{
			}
		}
	}
}
