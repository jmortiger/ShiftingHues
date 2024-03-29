﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using JMMGExt.Graphics;

namespace ShiftingHues.Library.Graphics
{
	/// <summary>
	/// Contains information about the sprites on a given <see cref="Texture2D"/>.
	/// </summary>
	public class SpriteSheet
	{
		#region Fields & Properties
		/// <summary>
		/// The spritesheet. - J Mor
		/// </summary>
		public Texture2D Texture { get; }

		public Rectangle[] SpriteSourceRects { get; private set; }

		/// <summary>
		/// The total number of sprites on this <see cref="SpriteSheet"/>. - J Mor
		/// </summary>
		public int SpritesInSheet { get => SpriteSourceRects.Length; }
		
		private Sprite[] sprites;
		public Sprite[] Sprites
		{
			get
			{
				if (sprites == null)
					InitializeSprites();
				return sprites;
			}
		}

		public Sprite this[int index]
		{
			get {
				if (sprites == null)
					InitializeSprites();
				return sprites[index];
			}
		}
		#endregion

		#region Constructors
		public SpriteSheet(
			Texture2D texture, 
			int rows, int columns, 
			int gridNodeWidth, int gridNodeHeight, 
			int spritesInSheet)
		{
			this.Texture = texture;
			
			InitializeArray(spritesInSheet, rows, columns, gridNodeWidth, gridNodeHeight);
		}

		public SpriteSheet(Texture2D texture, Rectangle[] spriteSourceRects)
		{
			this.Texture = texture;
			this.SpriteSourceRects = spriteSourceRects;
		}
		#endregion

		#region Methods
		/// <summary>
		/// Sets up an array with the bounds of each sprite in the sheet.
		/// </summary>
		private void InitializeArray(int spritesInSheet, int rows, int columns, int gridNodeWidth, int gridNodeHeight)
		{
			this.SpriteSourceRects = new Rectangle[spritesInSheet];
			int i;
			//for (int x = 0; x < columns; x++)
			//{
			//	for (int y = 0; y < rows; y++)
			for (int y = 0; y < rows; y++)
			{
				for (int x = 0; x < columns; x++)
				{
					i = x + (y * columns);
					if (i >= spritesInSheet)
						break;
					SpriteSourceRects[i] = new Rectangle(gridNodeWidth * x, gridNodeHeight * y, gridNodeWidth, gridNodeHeight);
				}
			}
		}

		/// <summary>
		/// Initializes the <see cref="Sprites"/>.
		/// </summary>
		private void InitializeSprites()
		{
			sprites = new Sprite[SpritesInSheet];
			//for (int i = 0; i < SpritesInSheet; i++)
			//	sprites[i] = new Sprite(this, i);
		}
		#endregion
	}
}
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;

//namespace SystemicDungeonCrawler
//{
//	/// <summary>
//	/// Handles the source rectangle of the current frame of the sprite & changing frames based on input. - J Mor
//	/// </summary>
//	class SpriteSheet
//	{
//		#region Fields & Properties
//		#region Fields
//		/// <summary>
//		/// The spritesheet. - J Mor
//		/// </summary>
//		private Texture2D sheet;
//		private Rectangle currImgSourceRectangle;
//		private int rows;
//		private int columns;
//		private int gridNodeWidth;
//		private int gridNodeHeight;
//		private int spritesInSheet;
//		private Point[] imgPositList;
//		#endregion

//		#region Properties
//		/// <summary>
//		/// The spritesheet. - J Mor
//		/// </summary>
//		public Texture2D Sheet { get => sheet; }
//		/// <summary>
//		/// The <see cref="Rectangle"/> that defines the location and size of the sprite. - J Mor
//		/// </summary>
//		public Rectangle CurrImgSourceRectangle { get => currImgSourceRectangle; }
//		/// <summary>
//		/// The number of rows on the <see cref="Sheet"/>. - J Mor
//		/// </summary>
//		public int Rows { get => rows; }
//		/// <summary>
//		/// The number of columns on the <see cref="Sheet"/>. - J Mor
//		/// </summary>
//		public int Columns { get => columns; }
//		/// <summary>
//		/// The width of each sprite on the <see cref="Sheet"/>. - J Mor
//		/// </summary>
//		public int GridNodeWidth { get => gridNodeWidth; }
//		/// <summary>
//		/// The height of each sprite on the <see cref="Sheet"/>. - J Mor
//		/// </summary>
//		public int GridNodeHeight { get => gridNodeHeight; }
//		/// <summary>
//		/// The total number of sprites on this <see cref="SpriteSheet"/>. - J Mor
//		/// </summary>
//		public int SpritesInSheet { get => spritesInSheet; }
//		/// <summary>
//		/// The current animation frame based on a 0 index. - J Mor
//		/// </summary>
//		public int CurrentFrame { get; set; }
//		/// <summary>
//		/// The origin of rotation (relative to the texture, not the screen). - J Mor
//		/// </summary>
//		public Vector2 OriginOfRotation { get; set; }
//		/// <summary>
//		/// The angle of rotation (in radians). - J Mor
//		/// </summary>
//		public float Rotation { get; set; }
//		/// <summary>
//		/// The x and y scaling factor (greater than 1 for growth, 1.0 for no change, less than 1 to shrink). - J Mor
//		/// </summary>
//		public Vector2 Scale { get; set; }
//		/// <summary>
//		/// The color mask. - J Mor
//		/// </summary>
//		public Color ColorMask { get; set; }
//		/// <summary>
//		/// The SpriteEffect to apply (flip horizontally, vertically, or not at all). - J Mor
//		/// </summary>
//		public SpriteEffects SpriteEffect { get; set; }
//		public float LayerDepth { get; set; }
//		#endregion
//		#endregion

//		#region Constructors
//		private SpriteSheet(int currentFrame, Vector2 originOfRotation, float rotation, Vector2 scale, Color colorMask, SpriteEffects spriteEffect, float layerDepth)
//		{
//			this.CurrentFrame = currentFrame;
//			this.OriginOfRotation = originOfRotation;
//			this.Rotation = rotation;
//			this.Scale = scale;
//			this.ColorMask = colorMask;
//			this.SpriteEffect = spriteEffect;
//			this.LayerDepth = layerDepth;
//		}
//		public SpriteSheet(Texture2D sheet, int rows, int columns, int gridNodeWidth, int gridNodeHeight, int spritesInSheet)
//			: this(0, Vector2.Zero, 0f, Vector2.One, Color.White, SpriteEffects.None, 0f)
//		{
//			this.sheet = sheet;
//			this.rows = rows;
//			this.columns = columns;
//			this.gridNodeWidth = gridNodeWidth;
//			this.gridNodeHeight = gridNodeHeight;
//			this.spritesInSheet = spritesInSheet;

//			this.currImgSourceRectangle = new Rectangle(0, 0, GridNodeWidth, GridNodeHeight);
//			this.imgPositList = new Point[SpritesInSheet];
//			InitializeArray();
//		}
//		public SpriteSheet(Texture2D sheet, int rows, int columns, int gridNodeWidth, int gridNodeHeight, int spritesInSheet, Vector2 originOfRotation, float rotation, Vector2 scale, Color colorMask, SpriteEffects spriteEffect, float layerDepth)
//			: this(0, originOfRotation, rotation, scale, colorMask, spriteEffect, layerDepth)
//		{
//			this.sheet = sheet;
//			this.rows = rows;
//			this.columns = columns;
//			this.gridNodeWidth = gridNodeWidth;
//			this.gridNodeHeight = gridNodeHeight;
//			this.spritesInSheet = spritesInSheet;

//			this.currImgSourceRectangle = new Rectangle(0, 0, GridNodeWidth, GridNodeHeight);
//			this.imgPositList = new Point[SpritesInSheet];
//			InitializeArray();
//		}
//		#endregion

//		#region Methods
//		/// <summary>
//		/// Sets up an array with the coordinates of each sprite in the sheet. - J Mor
//		/// </summary>
//		private void InitializeArray()
//		{
//			for (int x = 0; x < Rows; x++)
//			{
//				for (int y = 0; y < Columns; y++)
//				{
//					imgPositList[x + (y * Columns)] = new Point(GridNodeWidth * x, GridNodeHeight * y);
//				}
//			}
//		}
//		/// <summary>
//		/// Updates the <see cref="CurrImgSourceRectangle"/> according to the new value of <see cref="CurrentFrame"/>. - J Mor
//		/// </summary>
//		/// <exception cref="IndexOutOfRangeException">Thrown if the <see cref="CurrentFrame"/> is larger than the <see cref="SpritesInSheet"/>.</exception>
//		public void UpdateImgSourceRectangle()
//		{
//			if (CurrentFrame >= imgPositList.Length) throw new IndexOutOfRangeException();
//			else currImgSourceRectangle = new Rectangle(imgPositList[CurrentFrame], new Point(GridNodeWidth, GridNodeHeight));
//		}
//		/// <summary>
//		/// Updates the <see cref="CurrImgSourceRectangle"/> according to the new value of <see cref="CurrentFrame"/>. - J Mor
//		/// </summary>
//		/// <param name="frameNumber">The new value of <see cref="CurrentFrame"/>.</param>
//		public void UpdateImgSourceRectangle(int frameNumber)
//		{
//			CurrentFrame = frameNumber;
//			UpdateImgSourceRectangle();
//		}
//		#endregion
//	}
//}