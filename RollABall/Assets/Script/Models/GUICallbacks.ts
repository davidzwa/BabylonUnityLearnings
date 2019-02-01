
module PROJECT {

    // Somehow provide easy callbacks on the element in the GUIComponent
    export interface TextBlockInterface {
        observable: BABYLON.Observable<any>,
        updater: () => void
    }
}