﻿/* Babylon Mesh Component Template */

module PROJECT {
    export class ItemRotator extends BABYLON.MeshComponent {
        public constructor(owner: BABYLON.AbstractMesh, scene: BABYLON.Scene, tick: boolean = true, propertyBag: any = {}) {
            super(owner, scene, tick, propertyBag);
        }

        private speed:number = 0;

        protected ready() :void {
            // Scene execute when ready
        }

        protected start() :void {
            // Start component function
            this.speed = this.getProperty("speed", 0.01);
        }

        protected update() :void {
            // Update render loop function
            var delta:number = this.engine.getDeltaTime();
            this.mesh.rotate(new BABYLON.Vector3(1,1,1).multiplyByFloats(delta,delta,delta), this.speed);
        }

        protected after() :void {
            // After render loop function
        }

        protected destroy() :void {
            // Destroy component function
        }
    }
}