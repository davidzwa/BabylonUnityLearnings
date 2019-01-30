/* Babylon Mesh Component Template */

/// <reference path="../../Babylon/Library/babylon.d.ts" />
/// <reference path="../../Babylon/Library/babylon.manager.d.ts" />

module PROJECT {
    export class CubeComponent extends BABYLON.MeshComponent {
        owner: BABYLON.AbstractMesh;
        public constructor(owner: BABYLON.AbstractMesh, scene: BABYLON.Scene, tick: boolean = true, propertyBag: any = {}) {
            super(owner, scene, tick, propertyBag);
            this.owner = owner;
        }

        protected ready(): void {
            // Scene execute when ready
        }

        protected start(): void {
            // Start component function
            this.owner.position = new BABYLON.Vector3(this.rand(), this.rand(6), this.rand());
        }

        protected update(): void {
            // Update render loop function
        }

        protected after(): void {
            // After render loop function
        }

        protected destroy(): void {
            // Destroy component function
        }

        private rand(num: number = 10): number {
            return Math.round(Math.random() * num)
        }
    }
}