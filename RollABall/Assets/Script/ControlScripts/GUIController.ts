/* Babylon Scene Controller Template */
/// <reference path="../Models/GUIComponent.ts" />

module PROJECT {
    export class GUIController extends PROJECT.GUIComponent {
        GUIReference: string;
        titleText: string;
        titleName: string;
        counterText: string;
        counterName: string;
        counterScore: number = 0;

        public constructor(owner: BABYLON.AbstractMesh, scene: BABYLON.Scene, tick: boolean = true, propertyBag: any = {}) {
            super(owner, scene, tick, propertyBag);
        }

        protected start(): void {
            // Start component function
            this.titleText = this.getProperty("Title", 'Awesome game');
            this.titleName = this.getProperty("TitleReference", 'TitleName');
            this.counterText = this.getProperty("Counter", 'Score');
            this.counterName = this.getProperty("ScoreReference", 'ScoreName');

            this.setupGUI();
            this.addDefaultTitle({
                text: this.titleText,
                name: this.titleName,
                position: this.translate(VertPos.TOP, HorPos.MIDDLE, new Placement(100,0))
            });
            this.addBottomCounter({
                text: this.counterText,
                name: this.counterName,
                position: this.translate(VertPos.BOTTOM, HorPos.MIDDLE, new Placement(-100,0))
            }, this.counterScore);
        }

        public updateTitle(text: string) {
            (<BABYLON.GUI.TextBlock>this.controls[0]).text = text;
        }

        public updateScore(text: string) {
            (<BABYLON.GUI.TextBlock>this.controls[1]).text = this.getProperty("Counter", 'Score') + ": " + text;
        }

        // public registerObserver() {
            
        // }

    }

}