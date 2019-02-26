/* Babylon Scene Controller Template */
/// <reference path="../Models/GUIComponent.ts" />

module PROJECT {
    export class GUIController extends PROJECT.GUIComponent {
        GUIReference: string;
        private menuButton:BABYLON.GUI.Button;

        public constructor(owner: BABYLON.AbstractMesh, scene: BABYLON.Scene, tick: boolean = true, propertyBag: any = {}) {
            super(owner, scene, tick, propertyBag);
        }

        public static getGUI(scene: BABYLON.Scene) {
            return scene.getMeshByName("GUIController101").metadata.components[0].instance;
        }

        protected start(): void {
            this.setupGUI();
            this.addTitle({
                text: this.getProperty("Title", 'Awesome game'),
                name: this.getProperty("TitleName", 'TitleName'),
                position: this.translate(VertPos.TOP, HorPos.MIDDLE, new Placement(0, 100))
            });
            this.addCounter({
                text: this.getProperty("Score", 'Score'),
                name: this.getProperty("ScoreName", 'ScoreName'),
                position: this.translate(VertPos.BOTTOM, HorPos.MIDDLE, new Placement(0, -100))
            }, 0);
            this.menuButton = this.addMenuButtonWithObservable({
                text: this.getProperty("MenuButton", 'Menu'),
                name: this.getProperty("MenuButtonName", 'MenuButton'),
                position: this.translate(VertPos.BOTTOM, HorPos.LEFT, new Placement(200, -100)),
                width: 0.1,
                height: "40px"
            });

            this.getMenuButtonObservable().add((d, s) => {
                console.log(d,s);
                this.drawMenu();
            }, undefined, undefined, this);
        }

        // General Observable Hook into button
        public getMenuButtonObservable():BABYLON.Observable<any> {
            return this.menuButton.onPointerUpObservable;
        }

        // Draw Menu and transparent overlay
        public drawMenu() {
            this.addMenuOverlay();
        }

        // Draw game HUD elements
        public drawHUD() {

            alert('asd');
        }

        public updateTitle(text: string) {
            (<BABYLON.GUI.TextBlock>this.controls[0]).text = text;
        }

        public updateScore(text: string) {
            (<BABYLON.GUI.TextBlock>this.controls[1]).text = this.getProperty("Counter", 'Score') + ": " + text;
        }

    }

}