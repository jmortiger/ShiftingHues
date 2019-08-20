using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace ShiftingHues.Input.GamePadExt
{
	using DZMode = Microsoft.Xna.Framework.Input.GamePadDeadZone;
	/// <summary>
	/// A container for the deadzone information for a <see cref="GamePad"/>'s thumbsticks.
	/// </summary>
	public struct StickDeadzoneInfo
    {
        public DZMode InnerDZMode { get; private set; }
        public DZMode OuterDZMode { get; private set; }

        public LinearDeadzoneInfo DZInfoX { get; private set; }
        public LinearDeadzoneInfo DZInfoY { get; private set; }

        /// <summary>
        /// The max distance from the central X position that will register as the central X position.
        /// </summary>
        /// <example>If this is set to <c>0.25f</c>, stick positions from (-.25, 0) to (.25, 0) will be registered as (0, 0).</example>
        public float InnerDZX { get => DZInfoX.InnerDZ; }
        /// <summary>
        /// The max distance from the central Y position that will register as the central Y position.
        /// </summary>
        /// <example>If this is set to <c>0.25f</c>, stick positions from (0, -.25) to (0, .25) will be registered as (0, 0).</example>
        public float InnerDZY { get => DZInfoX.InnerDZ; }
        /// <summary>
        /// The max distance from the outer X position that will register as the outer X position.
        /// </summary>
        /// <example>If this is set to <c>0.25f</c>, stick positions from (-.75, 0) to (-1, 0) will be registered as (-1, 0), and likewise for positive values.</example>
        public float OuterDZX { get => 1 - DZInfoX.OuterDZ; }
        /// <summary>
        /// The max distance from the outer Y position that will register as the outer Y position.
        /// </summary>
        /// <example>If this is set to <c>0.25f</c>, stick positions from (0, -.75) to (0, -1) will be registered as (0, -1), and likewise for positive values.</example>
        public float OuterDZY { get => 1 - DZInfoX.OuterDZ; }

        ///// <summary>
        ///// The max distance from the central X position that will register as the central X position.
        ///// </summary>
        ///// <example>If this is set to <c>0.25f</c>, stick positions from (-.25, 0) to (.25, 0) will be registered as (0, 0).</example>
        //public float InnerDZX { get; private set; }
        ///// <summary>
        ///// The max distance from the central Y position that will register as the central Y position.
        ///// </summary>
        ///// <example>If this is set to <c>0.25f</c>, stick positions from (0, -.25) to (0, .25) will be registered as (0, 0).</example>
        //public float InnerDZY { get; private set; }
        ///// <summary>
        ///// The max distance from the outer X position that will register as the outer X position.
        ///// </summary>
        ///// <example>If this is set to <c>0.25f</c>, stick positions from (-.75, 0) to (-1, 0) will be registered as (-1, 0), and likewise for positive values.</example>
        //public float OuterDZX { get; private set; }
        ///// <summary>
        ///// The max distance from the outer Y position that will register as the outer Y position.
        ///// </summary>
        ///// <example>If this is set to <c>0.25f</c>, stick positions from (0, -.75) to (0, -1) will be registered as (0, -1), and likewise for positive values.</example>
        //public float OuterDZY { get; private set; }

        public StickDeadzoneInfo(
            DZMode InnerDZMode,
            DZMode OuterDZMode, 
            float InnerDZX, 
            float InnerDZY, 
            float OuterDZX, 
            float OuterDZY)
        {
            this.InnerDZMode = InnerDZMode;
            this.OuterDZMode = OuterDZMode;
            InnerDZX = Math.Abs(InnerDZX);
            InnerDZY = Math.Abs(InnerDZY);
            OuterDZX = Math.Abs(OuterDZX);
            OuterDZY = Math.Abs(OuterDZY);
            DZInfoX = new LinearDeadzoneInfo(InnerDZX, OuterDZX);
            DZInfoY = new LinearDeadzoneInfo(InnerDZY, OuterDZY);
        }

        public StickDeadzoneInfo(
            float InnerDZX,
            float InnerDZY,
            float OuterDZX,
            float OuterDZY)
        {
            this.InnerDZMode = DZMode.IndependentAxes;
            this.OuterDZMode = DZMode.IndependentAxes;
            InnerDZX = Math.Abs(InnerDZX);
            InnerDZY = Math.Abs(InnerDZY);
            OuterDZX = Math.Abs(OuterDZX);
            OuterDZY = Math.Abs(OuterDZY);
            DZInfoX = new LinearDeadzoneInfo(InnerDZX, OuterDZX);
            DZInfoY = new LinearDeadzoneInfo(InnerDZY, OuterDZY);
        }

        public Vector2 GetStickVal(float x, float y)
        {
            // 1. Apply linear DZs
            x = DZInfoX.GetVal(x);
            y = DZInfoY.GetVal(y);

            //// 2. Check the modes
            //// If it's circular...
            //if (InnerDZMode == DZMode.Circular)
            //{ // ... scale the coordinates to a circular path (i.e. Length = 1)
            //    if ()
            //}
            // TODO: finish implementation
            return new Vector2(x, y);
            //throw new NotImplementedException();
        }

        public bool HasInnerDeadzone() => (InnerDZMode == DZMode.None || (InnerDZX == 0 && InnerDZY == 0));

        public bool HasOuterDeadzone() => (OuterDZMode == DZMode.None || (OuterDZX == 0 && OuterDZY == 0));
    }

    public struct LinearDeadzoneInfo
    {
        /// <summary>
        /// The max value above the mininum value that will register as the mininum value.
        /// </summary>
        /// <example>If this is set to <c>0.25f</c>, values from 0 to .25 will be registered as 0.</example>
        public float InnerDZ;
        /// <summary>
        /// The min value below the maximum value that will register as the maximum value.
        /// </summary>
        /// <example>If this is set to <c>0.75f</c>, values from .75 to 1 will be registered as 1.</example>
        public float OuterDZ;

        public LinearDeadzoneInfo(float InnerDZ, float OuterDZ)
        {
            this.InnerDZ = InnerDZ;
            this.OuterDZ = OuterDZ;
        }

        /// <summary>
        /// Applies deadzones to a value between 0 and 1, inclusive.
        /// </summary>
        /// <param name="origVal">The value to apply the deadzones to.</param>
        /// <returns>The value scaled to the deadzones.</returns>
        /// <remarks>Expects a positive float from 0 to 1 inclusive. Will clamp value.</remarks>
        public float GetValAbs(float origVal)
        {
            float origMin = 0, origMax = 1;
            origVal = MathHelper.Clamp(origVal, origMin, origMax);
            // Edge Cases
            // 1. Val is outside of inner bound; val is origMin
            if (origVal < InnerDZ) return origMin;
            // 2. Val is outside of outer bound; val is origMax
            if (origVal > OuterDZ) return origMax;
            // End of edge cases

            float origRange = origMax - origMin,
                  newRange = OuterDZ - InnerDZ;
            // Get how far along the origVal was along the newRange by subtracting the InnerDZ and expressing that as a fraction of the newRange.
            float scaledDistFromInnerDZ = (origVal - InnerDZ) / newRange;
            // It's now independent of the original scale, so define it in the orig scale by multiplying by the origRange, then add it to the origMin to get the new val.
            return (scaledDistFromInnerDZ * origRange) + origMin;
        }

        /// <summary>
        /// TODO: Finish docs
        /// </summary>
        /// <param name="origVal"></param>
        /// <returns></returns>
        public float GetVal(float origVal)
        {
            return (origVal >= 0) ? GetValAbs(origVal) : -1 * GetValAbs(-1 * origVal);
        }

        /// <summary>
        /// TODO: Finish Doc
        /// </summary>
        /// <param name="origVal"></param>
        /// <param name="origMin">The minimum positive (or 0) value of <paramref name="origVal"/>.</param>
        /// <param name="origMax">The maximum positive value of <paramref name="origVal"/>.</param>
        /// <returns></returns>
        /// <remarks>Assumes <paramref name="origMax"/> and <paramref name="origMin"/> are positive. Behviour for this not happening is not expected.</remarks>
        public float GetVal(float origVal, float origMin, float origMax)
        {
            return (origVal >= 0) ? GetValAbs(origVal, origMin, origMax) : -1 * GetValAbs(-1 * origVal, origMin, origMax);
        }

        /// <summary>
        /// TODO: Finish docs
        /// </summary>
        /// <param name="origVal"></param>
        /// <param name="origMin"></param>
        /// <param name="origMax"></param>
        /// <returns></returns>
        public float GetValAbs(float origVal, float origMin, float origMax)
        {
            origVal = MathHelper.Clamp(origVal, origMin, origMax);
            // Edge Cases
            // 1. Val is outside of inner bound; val is origMin
            if (origVal < InnerDZ) return origMin;
            // 2. Val is outside of outer bound; val is origMax
            if (origVal > OuterDZ) return origMax;
            // End of edge cases

            float origRange = origMax - origMin,
                  newRange = OuterDZ - InnerDZ;
            // Get how far along the origVal was along the newRange by subtracting the InnerDZ and expressing that as a fraction of the newRange.
            float scaledDistFromInnerDZ = (origVal - InnerDZ) / newRange;
            // It's now independent of the original scale, so define it in the orig scale by multiplying by the origRange, then add it to the origMin to get the new val.
            return (scaledDistFromInnerDZ * origRange) + origMin;
        }
    }

    public struct GamePadValues
    {
        #region Fields and Properties
        public float LStickX { get; private set; }
        public float LStickY { get; private set; }
        public float RStickX { get; private set; }
        public float RStickY { get; private set; }
        public float LTrigger { get; private set; }
        public float RTrigger { get; private set; }
        public StickDeadzoneInfo LStickDZs { get; private set; }
        public StickDeadzoneInfo RStickDZs { get; private set; }
        #endregion

        #region Constructors

        public GamePadValues(
            float lStickX, 
            float lStickY, 
            float rStickX, 
            float rStickY, 
            float lTrigger, 
            float rTrigger,
            StickDeadzoneInfo LStickDZs,
            StickDeadzoneInfo RStickDZs)
        {
            this.LStickX = lStickX;
            this.LStickY = lStickY;
            this.RStickX = rStickX;
            this.RStickY = rStickY;
            this.LTrigger = lTrigger;
            this.RTrigger = rTrigger;

            this.LStickDZs = LStickDZs;
            this.RStickDZs = RStickDZs;
        }

        public GamePadValues(float lStickX, float lStickY, float rStickX, float rStickY, float lTrigger, float rTrigger)
        {
            this.LStickX  = lStickX;
            this.LStickY  = lStickY;
            this.RStickX  = rStickX;
            this.RStickY  = rStickY;
            this.LTrigger = lTrigger;
            this.RTrigger = rTrigger;

            this.LStickDZs = new StickDeadzoneInfo(DZMode.None, DZMode.None, 0, 0, 0, 0);
            this.RStickDZs = new StickDeadzoneInfo(DZMode.None, DZMode.None, 0, 0, 0, 0);
        }

        public GamePadValues(GamePadThumbSticks thumbSticks, GamePadTriggers triggers)
            : this(
                  thumbSticks.Left.X, thumbSticks.Left.Y, 
                  thumbSticks.Right.X, thumbSticks.Right.Y, 
                  triggers.Left, triggers.Right)
        {

        }
        #endregion

        #region Methods

        #endregion
    }
}