﻿using System;
using BEPUphysics.BroadPhaseEntries;
using BEPUphysics.Entities;
using BEPUphysics.Entities.Prefabs;

using BEPUphysics.CollisionRuleManagement;
using BEPUphysics.Materials;
using BEPUutilities;
using FixMath.NET;
using Fix64 = TrueSync.FP;

namespace BEPUphysics.Vehicle
{
    /// <summary>
    /// Superclass for the shape of the tires of a vehicle.
    /// Responsible for figuring out where the wheel touches the ground and
    /// managing graphical properties.
    /// </summary>
    public abstract class WheelShape : ICollisionRulesOwner
    {
        private TrueSync.FP airborneWheelAcceleration = (TrueSync.FP)40;


        private TrueSync.FP airborneWheelDeceleration = (TrueSync.FP)4;
        private TrueSync.FP brakeFreezeWheelDeceleration = (TrueSync.FP)40;

        /// <summary>
        /// Collects collision pairs from the environment.
        /// </summary>
        protected internal Box detector = new Box(Vector3.Zero, F64.C0, F64.C0, F64.C0);

        protected internal Matrix localGraphicTransform;
        protected TrueSync.FP spinAngle;


        protected TrueSync.FP spinVelocity;
        internal TrueSync.FP steeringAngle;

        internal Matrix steeringTransform;
        protected internal Wheel wheel;

        protected internal Matrix worldTransform;

        CollisionRules collisionRules = new CollisionRules() { Group = CollisionRules.DefaultDynamicCollisionGroup };
        /// <summary>
        /// Gets or sets the collision rules used by the wheel.
        /// </summary>
        public CollisionRules CollisionRules
        {
            get { return collisionRules; }
            set { collisionRules = value; }
        }

        /// <summary>
        /// Gets or sets the graphical radius of the wheel.
        /// </summary>
        public abstract TrueSync.FP Radius { get; set; }

        /// <summary>
        /// Gets or sets the rate at which the wheel's spinning velocity increases when accelerating and airborne.
        /// This is a purely graphical effect.
        /// </summary>
        public TrueSync.FP AirborneWheelAcceleration
        {
            get { return airborneWheelAcceleration; }
            set { airborneWheelAcceleration = TrueSync.FP.Abs(value); }
        }

        /// <summary>
        /// Gets or sets the rate at which the wheel's spinning velocity decreases when the wheel is airborne and its motor is idle.
        /// This is a purely graphical effect.
        /// </summary>
        public TrueSync.FP AirborneWheelDeceleration
        {
            get { return airborneWheelDeceleration; }
            set { airborneWheelDeceleration = TrueSync.FP.Abs(value); }
        }

        /// <summary>
        /// Gets or sets the rate at which the wheel's spinning velocity decreases when braking.
        /// This is a purely graphical effect.
        /// </summary>
        public TrueSync.FP BrakeFreezeWheelDeceleration
        {
            get { return brakeFreezeWheelDeceleration; }
            set { brakeFreezeWheelDeceleration = TrueSync.FP.Abs(value); }
        }

        /// <summary>
        /// Gets the detector entity used by the wheelshape to collect collision pairs.
        /// </summary>
        public Box Detector
        {
            get { return detector; }
        }

        /// <summary>
        /// Gets or sets whether or not to halt the wheel spin while the WheelBrake is active.
        /// </summary>
        public bool FreezeWheelsWhileBraking { get; set; }

        /// <summary>
        /// Gets or sets the local graphic transform of the wheel shape.
        /// This transform is applied first when creating the shape's worldTransform.
        /// </summary>
        public Matrix LocalGraphicTransform
        {
            get { return localGraphicTransform; }
            set { localGraphicTransform = value; }
        }

        /// <summary>
        /// Gets or sets the current spin angle of this wheel.
        /// This changes each frame based on the relative velocity between the
        /// support and the wheel.
        /// </summary>
        public TrueSync.FP SpinAngle
        {
            get { return spinAngle; }
            set { spinAngle = value; }
        }

        /// <summary>
        /// Gets or sets the graphical spin velocity of the wheel based on the relative velocity 
        /// between the support and the wheel.  Whenever the wheel is in contact with
        /// the ground, the spin velocity will be each frame.
        /// </summary>
        public TrueSync.FP SpinVelocity
        {
            get { return spinVelocity; }
            set { spinVelocity = value; }
        }

        /// <summary>
        /// Gets or sets the current steering angle of this wheel.
        /// </summary>
        public TrueSync.FP SteeringAngle
        {
            get { return steeringAngle; }
            set { steeringAngle = value; }
        }

        /// <summary>
        /// Gets the wheel object associated with this shape.
        /// </summary>
        public Wheel Wheel
        {
            get { return wheel; }
            internal set { wheel = value; }
        }

        /// <summary>
        /// Gets the world matrix of the wheel for positioning a graphic.
        /// </summary>
        public Matrix WorldTransform
        {
            get { return worldTransform; }
        }


        /// <summary>
        /// Updates the wheel's world transform for graphics.
        /// Called automatically by the owning wheel at the end of each frame.
        /// If the engine is updating asynchronously, you can call this inside of a space read buffer lock
        /// and update the wheel transforms safely.
        /// </summary>
        public abstract void UpdateWorldTransform();


        internal void OnAdditionToSpace(Space space)
        {
            detector.CollisionInformation.collisionRules.Specific.Add(wheel.vehicle.Body.CollisionInformation.collisionRules, CollisionRule.NoBroadPhase);
            detector.CollisionInformation.collisionRules.Personal = CollisionRule.NoNarrowPhaseUpdate;
            detector.CollisionInformation.collisionRules.group = CollisionRules.DefaultDynamicCollisionGroup;
            //Need to put the detectors in appropriate locations before adding, or else the broad phase would see objects at (0,0,0) and make things gross.
            UpdateDetectorPosition();
            space.Add(detector);

        }

        internal void OnRemovalFromSpace(Space space)
        {
            space.Remove(detector);
            detector.CollisionInformation.CollisionRules.Specific.Remove(wheel.vehicle.Body.CollisionInformation.collisionRules);
        }

        /// <summary>
        /// Updates the spin velocity and spin angle for the shape.
        /// </summary>
        /// <param name="dt">Simulation timestep.</param>
        internal void UpdateSpin(TrueSync.FP dt)
        {
            if (wheel.HasSupport && !(wheel.brake.IsBraking && FreezeWheelsWhileBraking))
            {
                //On the ground, not braking.
                spinVelocity = wheel.drivingMotor.RelativeVelocity / Radius;
            }
            else if (wheel.HasSupport && wheel.brake.IsBraking && FreezeWheelsWhileBraking)
            {
                //On the ground, braking
                TrueSync.FP deceleratedValue = F64.C0;
                if (spinVelocity > F64.C0)
                    deceleratedValue = MathHelper.Max(spinVelocity - brakeFreezeWheelDeceleration * dt, F64.C0);
                else if (spinVelocity < F64.C0)
                    deceleratedValue = MathHelper.Min(spinVelocity + brakeFreezeWheelDeceleration * dt, F64.C0);

                spinVelocity = wheel.drivingMotor.RelativeVelocity / Radius;

                if (TrueSync.FP.Abs(deceleratedValue) < TrueSync.FP.Abs(spinVelocity))
                    spinVelocity = deceleratedValue;
            }
            else if (!wheel.HasSupport && wheel.drivingMotor.TargetSpeed != F64.C0)
            {
                //Airborne and accelerating, increase spin velocity.
                TrueSync.FP maxSpeed = TrueSync.FP.Abs(wheel.drivingMotor.TargetSpeed) / Radius;
                spinVelocity = MathHelper.Clamp(spinVelocity + TrueSync.FP.Sign(wheel.drivingMotor.TargetSpeed) * airborneWheelAcceleration * dt, -maxSpeed, maxSpeed);
            }
            else if (!wheel.HasSupport && wheel.Brake.IsBraking)
            {
                //Airborne and braking
                if (spinVelocity > F64.C0)
                    spinVelocity = MathHelper.Max(spinVelocity - brakeFreezeWheelDeceleration * dt, F64.C0);
                else if (spinVelocity < F64.C0)
                    spinVelocity = MathHelper.Min(spinVelocity + brakeFreezeWheelDeceleration * dt, F64.C0);
            }
            else if (!wheel.HasSupport)
            {
                //Just idly slowing down.
                if (spinVelocity > F64.C0)
                    spinVelocity = MathHelper.Max(spinVelocity - airborneWheelDeceleration * dt, F64.C0);
                else if (spinVelocity < F64.C0)
                    spinVelocity = MathHelper.Min(spinVelocity + airborneWheelDeceleration * dt, F64.C0);
            }
            spinAngle += spinVelocity * dt;
        }

        /// <summary>
        /// Finds a supporting entity, the contact location, and the contact normal.
        /// </summary>
        /// <param name="location">Contact point between the wheel and the support.</param>
        /// <param name="normal">Contact normal between the wheel and the support.</param>
        /// <param name="suspensionLength">Length of the suspension at the contact.</param>
        /// <param name="supportCollidable">Collidable supporting the wheel, if any.</param>
        /// <param name="entity">Entity supporting the wheel, if any.</param>
        /// <param name="material">Material of the support.</param>
        /// <returns>Whether or not any support was found.</returns>
        protected internal abstract bool FindSupport(out Vector3 location, out Vector3 normal, out TrueSync.FP suspensionLength, out Collidable supportCollidable, out Entity entity, out Material material);

        /// <summary>
        /// Initializes the detector entity and any other necessary logic.
        /// </summary>
        protected internal abstract void Initialize();

        /// <summary>
        /// Updates the position of the detector before each step.
        /// </summary>
        protected internal abstract void UpdateDetectorPosition();

    }
}