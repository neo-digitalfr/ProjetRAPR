using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Collections;
using System.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;

//Even though they are used like normal methods, extension
//methods must be declared static. Notice that the first
//parameter has the 'this' keyword followed by the right type
//variable. This variable denotes which class the extension
//method becomes a part of.


/// <summary>
/// Extension methods.
/// </summary>
namespace ExtensionMethods
{
	/// <summary>
	/// Extension methods. Contains all extension methods usefull in the project
	/// </summary>
	public static class ExtensionMethods  
	{

		/// <summary>
		/// Gets the copy of a component, used for the popcorn refactor
		/// </summary>
		/// <returns>The copy of.</returns>
		/// <param name="comp">Comp.</param>
		/// <param name="other">Other.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public static T GetCopyOf<T>(this Component comp, T other) where T : Component
		{
			Type type = other.GetType();
			//if (type != other.GetType()) return null; // type mis-match
			BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Default | BindingFlags.DeclaredOnly;
			PropertyInfo[] pinfos = type.GetProperties(flags);
			foreach (var pinfo in pinfos) {
				if (pinfo.CanWrite) {
					try {
						pinfo.SetValue(comp, pinfo.GetValue(other, null), null);
					}
					catch { } // In case of NotImplementedException being thrown. For some reason specifying that exception didn't seem to catch it, so I didn't catch anything specific.
				}
			}
			FieldInfo[] finfos = type.GetFields(flags);
			foreach (var finfo in finfos) {
				finfo.SetValue(comp, finfo.GetValue(other));
			}
			return comp as T;
		}



		#region List


		/*
		public static List<MDL_RankPlayer> SortByRank(this List<MDL_RankPlayer> _lCarPieces)
		{
			return _lCarPieces.OrderBy(o=>o.m_iPosition).ToList();
		}

		public static MDL_CarPiece GetCarPiece(this List<MDL_CarPiece> _lCarPieces,string _sId )
		{
			return _lCarPieces.Find(carPiece => carPiece.m_sID == _sId);
		}

		public static CarPieceHolder GetCarPiece(this List<CarPieceHolder> _lCarPieces,string _sId )
		{
			return _lCarPieces.Find(carPiece => carPiece.m_carPiece.m_sID == _sId);
		}*/
		#endregion

		#region GameObject
		/// <summary>
		/// Sets active only if state is different
		/// </summary>
		/// <param name="_go">_go.</param>
		/// <param name="_b">If set to <c>true</c> _b.</param>
		public static void SetActiveSmart(this GameObject _go, bool _b)
		{
			if(_go.activeInHierarchy != _b)
				_go.SetActive(_b);
		}
		#endregion

		#region Transform
		public static Canvas FindCanvasInParents(this Transform _transform)
		{
			Canvas canvas = _transform.GetComponent<Canvas>();
			if(canvas!=null) 
				return canvas;
			else if(_transform.parent!=null)
					return _transform.parent.FindCanvasInParents();
			else
				return null;
		}


		/// <summary>
		/// Resets the transformation.
		/// </summary>
		/// <param name="_transform">_transform.</param>
		public static void ResetTransformation(this Transform _transform)
		{
			_transform.position = Vector3.zero;
			_transform.localRotation = Quaternion.identity;
			_transform.localScale = new Vector3(1, 1, 1);
		}

		/// <summary>
		/// Looks the at 2d.
		/// </summary>
		/// <param name="_transform">_transform.</param>
		/// <param name="_target">_target.</param>
		public static void LookAt2D(this Transform _transform, Transform _target) 
		{
			Vector3 relative = _transform.InverseTransformPoint(_target.position);
			float angle = Mathf.Atan2(relative.x, relative.y) * Mathf.Rad2Deg;
			_transform.Rotate(0,0, -angle, Space.Self);
		}

		/// <summary>
		/// Looks the at 2d.
		/// </summary>
		/// <param name="_transform">_transform.</param>
		/// <param name="pos">Position.</param>
		public static void LookAt2D(this Transform _transform, Vector3 pos) 
		{
			Vector3 relative = _transform.InverseTransformPoint(pos);
			float angle = Mathf.Atan2(relative.x, relative.y) * Mathf.Rad2Deg;
			_transform.Rotate(0,0, -angle, Space.Self);
		}


		/// <summary>
		/// Sets the position x.
		/// </summary>
		/// <param name="_transform">_transform.</param>
		/// <param name="_newX">_new x.</param>
		public static void SetPositionX(this Transform _transform, float _newX)
		{
			_transform.position = new Vector3(_newX, _transform.position.y, _transform.position.z);
		}

		/// <summary>
		/// Sets the position y.
		/// </summary>
		/// <param name="_transform">_transform.</param>
		/// <param name="_newY">_new y.</param>
		public static void SetPositionY(this Transform _transform, float _newY)
		{
			_transform.position = new Vector3(_transform.position.x, _newY, _transform.position.z);
		}

		/// <summary>
		/// Sets the position z.
		/// </summary>
		/// <param name="_transform">_transform.</param>
		/// <param name="_newZ">_new z.</param>
		public static void SetPositionZ(this Transform _transform, float _newZ)
		{
			_transform.position = new Vector3(_transform.position.x, _transform.position.y, _newZ);
		}

		/// <summary>
		/// Finds all child.
		/// </summary>
		/// <returns>The all child.</returns>
		/// <param name="_transform">_transform.</param>
		/// <param name="_sName">_s name.</param>
		public static Transform[] FindAllChild(this Transform _transform, string _sName)
		{
			List<Transform> lTrsf = new List<Transform>();
			int iCount = _transform.childCount;
			Transform trsfChild;
			for(int i=0;i<iCount;++i)
			{
				trsfChild = _transform.GetChild(i);
				if(trsfChild.name == _sName)
				{
					lTrsf.Add(trsfChild);
				}
			}
			return lTrsf.ToArray();
		}
		#endregion


		#region Sprite
		/// <summary>
		/// Sets the alpha only
		/// </summary>
		/// <param name="_spriteRdr">_sprite rdr.</param>
		/// <param name="_fAlpha">_f alpha.</param>
		public static void SetAlpha(this SpriteRenderer _spriteRdr, float _fAlpha)
		{
			Color col = _spriteRdr.color;
			col.a = _fAlpha;
			_spriteRdr.color = col;
		}
		#endregion


		#region UnityEvent
		/// <summary>
		/// Safes invoke. check if not null 
		/// </summary>
		/// <param name="_evt">_evt.</param>
		public static void SafeInvoke(this UnityEvent _evt)
		{
			if(_evt!=null)
			{
				_evt.Invoke();
			}
		}
		#endregion

		#region object
		/// <summary>
		/// Determines if is type of the specified _object _type.
		/// </summary>
		/// <returns><c>true</c> if is type of the specified _object _type; otherwise, <c>false</c>.</returns>
		/// <param name="_object">_object.</param>
		/// <param name="_type">_type.</param>
		public static bool IsTypeOf(this object _object, System.Type _type)
		{
			if(_object.GetType() == _type)
			{
				return true;
			}
			return false;
		}
		#endregion

		#region Image
		/// <summary>
		/// Updates the progress bar.
		/// </summary>
		/// <param name="_img">_img.</param>
		/// <param name="_fValue">_f value.</param>
		public static void UpdateProgressBar(this Image _img, float _fValue)
		{
			_img.fillAmount = _fValue;
		}

		/// <summary>
		/// Updates the flat progress bar.
		/// </summary>
		/// <param name="_img">_img.</param>
		/// <param name="_fValue">_f value.</param>
		/// <param name="_barEnd">_bar end.</param>
		public static void UpdateFlatProgressBar(this Image _img,float _fValue, RectTransform _barEnd )
		{
			_img.fillAmount = _fValue;
			RectTransform rctImage = _img.GetComponent<RectTransform>();
            if ( _img.fillMethod == Image.FillMethod.Horizontal )
                _barEnd.localPosition = new Vector2(rctImage.rect.position.x + (rctImage.rect.width * _fValue), 0);
            else if ( _img.fillMethod == Image.FillMethod.Vertical )
                _barEnd.localPosition = new Vector2(0, rctImage.rect.position.y + (rctImage.rect.height * _fValue));
		}

		#endregion

        #region Rect Transform
		/// <summary>
		/// Sets the z and update its scale according to its current z value.
		/// </summary>
		/// <param name="_transform">_transform.</param>
		/// <param name="_fZ">float z.</param>
		public static void SetZ(this RectTransform _transform, float _fZ)
		{
			if(!Mathf.Approximately(_fZ , _transform.localPosition.z))
			{
				float fDiff = 2 * (_fZ - _transform.localPosition.z);
				if(_fZ  < _transform.localPosition.z)
					fDiff = -(1/fDiff);
				_transform.localScale = _transform.localScale * fDiff;
				_transform.localPosition = new Vector3(_transform.localPosition.x, _transform.localPosition.y, _fZ);
			}
		}

		/// <summary>
		/// Sets the default scale.
		/// </summary>
		/// <param name="trans">Trans.</param>
        public static void SetDefaultScale( this RectTransform trans )
        {
            trans.localScale = new Vector3( 1, 1, 1 );
        }

		/// <summary>
		/// Sets the pivot and anchors.
		/// </summary>
		/// <param name="trans">Trans.</param>
		/// <param name="aVec">A vec.</param>
        public static void SetPivotAndAnchors( this RectTransform trans, Vector2 aVec )
        {
            trans.pivot = aVec;
            trans.anchorMin = aVec;
            trans.anchorMax = aVec;
        }

		/// <summary>
		/// Gets the size.
		/// </summary>
		/// <returns>The size.</returns>
		/// <param name="trans">Trans.</param>
        public static Vector2 GetSize( this RectTransform trans )
        {
            return trans.rect.size;
        }

		/// <summary>
		/// Gets the width.
		/// </summary>
		/// <returns>The width.</returns>
		/// <param name="trans">Trans.</param>
        public static float GetWidth( this RectTransform trans )
        {
            return trans.rect.width;
        }

		/// <summary>
		/// Gets the height.
		/// </summary>
		/// <returns>The height.</returns>
		/// <param name="trans">Trans.</param>
        public static float GetHeight( this RectTransform trans )
        {
            return trans.rect.height;
        }

		/// <summary>
		/// Sets the position of pivot.
		/// </summary>
		/// <param name="trans">Trans.</param>
		/// <param name="newPos">New position.</param>
        public static void SetPositionOfPivot( this RectTransform trans, Vector2 newPos )
        {
            trans.localPosition = new Vector3( newPos.x, newPos.y, trans.localPosition.z );
        }

		/// <summary>
		/// Sets the left bottom position.
		/// </summary>
		/// <param name="trans">Trans.</param>
		/// <param name="newPos">New position.</param>
        public static void SetLeftBottomPosition( this RectTransform trans, Vector2 newPos )
        {
            trans.localPosition = new Vector3( newPos.x + (trans.pivot.x * trans.rect.width), newPos.y + (trans.pivot.y * trans.rect.height), trans.localPosition.z );
        }

		/// <summary>
		/// Sets the left top position.
		/// </summary>
		/// <param name="trans">Trans.</param>
		/// <param name="newPos">New position.</param>
        public static void SetLeftTopPosition( this RectTransform trans, Vector2 newPos )
        {
            trans.localPosition = new Vector3( newPos.x + (trans.pivot.x * trans.rect.width), newPos.y - ((1f - trans.pivot.y) * trans.rect.height), trans.localPosition.z );
        }

		/// <summary>
		/// Sets the right bottom position.
		/// </summary>
		/// <param name="trans">Trans.</param>
		/// <param name="newPos">New position.</param>
        public static void SetRightBottomPosition( this RectTransform trans, Vector2 newPos )
        {
            trans.localPosition = new Vector3( newPos.x - ((1f - trans.pivot.x) * trans.rect.width), newPos.y + (trans.pivot.y * trans.rect.height), trans.localPosition.z );
        }

		/// <summary>
		/// Sets the right top position.
		/// </summary>
		/// <param name="trans">Trans.</param>
		/// <param name="newPos">New position.</param>
        public static void SetRightTopPosition( this RectTransform trans, Vector2 newPos )
        {
            trans.localPosition = new Vector3( newPos.x - ((1f - trans.pivot.x) * trans.rect.width), newPos.y - ((1f - trans.pivot.y) * trans.rect.height), trans.localPosition.z );
        }

		/// <summary>
		/// Sets the size.
		/// </summary>
		/// <param name="trans">Trans.</param>
		/// <param name="newSize">New size.</param>
        public static void SetSize( this RectTransform trans, Vector2 newSize )
        {
            Vector2 oldSize = trans.rect.size;
            Vector2 deltaSize = newSize - oldSize;
            trans.offsetMin = trans.offsetMin - new Vector2( deltaSize.x * trans.pivot.x, deltaSize.y * trans.pivot.y );
            trans.offsetMax = trans.offsetMax + new Vector2( deltaSize.x * (1f - trans.pivot.x), deltaSize.y * (1f - trans.pivot.y) );
        }

		/// <summary>
		/// Sets the width.
		/// </summary>
		/// <param name="trans">Trans.</param>
		/// <param name="newSize">New size.</param>
        public static void SetWidth( this RectTransform trans, float newSize )
        {
            SetSize( trans, new Vector2( newSize, trans.rect.size.y ) );
        }

		/// <summary>
		/// Sets the height.
		/// </summary>
		/// <param name="trans">Trans.</param>
		/// <param name="newSize">New size.</param>
        public static void SetHeight( this RectTransform trans, float newSize )
        {
            SetSize( trans, new Vector2( trans.rect.size.x, newSize ) );
        }
        #endregion
    }
}
