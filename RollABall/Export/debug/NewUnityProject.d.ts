/// <reference path="../../Assets/Babylon/Library/babylon.d.ts" />
/// <reference path="../../Assets/Babylon/Library/babylon.manager.d.ts" />
declare module PROJECT {
    class BallCamera extends BABYLON.CameraComponent {
        constructor(owner: BABYLON.Camera, scene: BABYLON.Scene, tick?: boolean, propertyBag?: any);
        private mesh;
        private offset;
        protected start(): void;
        protected update(): void;
        protected after(): void;
        protected destroy(): void;
    }
}
declare module PROJECT {
    class GUIController extends BABYLON.MeshComponent {
        title: string;
        textBlock: BABYLON.GUI.TextBlock;
        GUI: BABYLON.GUI.AdvancedDynamicTexture;
        playerCount: number;
        constructor(owner: BABYLON.AbstractMesh, scene: BABYLON.Scene, tick?: boolean, propertyBag?: any);
        protected ready(): void;
        protected start(): void;
        protected update(): void;
        protected after(): void;
        protected destroy(): void;
    }
}
declare module PROJECT {
    class Player extends BABYLON.MeshComponent {
        count: number;
        speed: number;
        items: BABYLON.Mesh[];
        constructor(owner: BABYLON.AbstractMesh, scene: BABYLON.Scene, tick?: boolean, propertyBag?: any);
        protected ready(): void;
        protected start(): void;
        protected update(): void;
        private updatePlayerMovement;
        private updatePickupCollisions;
        private updateCollectionCount;
        protected after(): void;
        protected destroy(): void;
    }
}
declare module PROJECT {
    class BallScene extends BABYLON.MeshComponent {
        constructor(owner: BABYLON.AbstractMesh, scene: BABYLON.Scene, tick?: boolean, propertyBag?: any);
        protected ready(): void;
        protected start(): void;
        protected update(): void;
        protected after(): void;
        protected destroy(): void;
    }
}
declare module PROJECT {
    class CubeComponent extends BABYLON.MeshComponent {
        owner: BABYLON.AbstractMesh;
        constructor(owner: BABYLON.AbstractMesh, scene: BABYLON.Scene, tick?: boolean, propertyBag?: any);
        protected ready(): void;
        protected start(): void;
        protected update(): void;
        protected after(): void;
        protected destroy(): void;
        private rand;
    }
}
declare module PROJECT {
    class ItemRotator extends BABYLON.MeshComponent {
        constructor(owner: BABYLON.AbstractMesh, scene: BABYLON.Scene, tick?: boolean, propertyBag?: any);
        private speed;
        protected ready(): void;
        protected start(): void;
        protected update(): void;
        protected after(): void;
        protected destroy(): void;
    }
}
