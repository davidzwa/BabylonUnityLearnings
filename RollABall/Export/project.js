if (!navigator.project) {
    navigator.project = {};
    navigator.project.rootPath = null;
    navigator.project.tagName = "NewUnityProject.babylon.js";
    navigator.project.scenePath = "scenes/";
    navigator.project.scriptPath = "scripts/";
    navigator.project.projectTitle = "New Unity Project";
    navigator.project.defaultScene = "BallScene.babylon";
    navigator.project._scriptLibraries = [];
    navigator.project._loadProjectScripts = function (scripts, index, tag) {
        var header = document.head || document.getElementsByTagName("head")[0];        
        if (scripts[index]) {
            var filesrc = navigator.project.rootPath + scripts[index];
            var fileref = document.createElement("script");
            fileref.setAttribute("type", "text/javascript");
            fileref.setAttribute("src", filesrc);
            fileref.onerror = function(err) {
                console.warn("Failed to load script for " + filesrc + ". " + err.message);
                index = index + 1;
                navigator.project._loadProjectScripts(scripts, index, tag)
            };
            fileref.onload = function() {
                index = index + 1;
                navigator.project._loadProjectScripts(scripts, index, tag)
            };
            header.appendChild(fileref)
        } else {
            var loaded = document.createEvent("HTMLEvents");
            loaded.initEvent("projectload", true, true);
            document.dispatchEvent(loaded);
        }
    };
    navigator.project._findProjectPath = function () {
        var path = null;
        var scripts = document.getElementsByTagName('script');
        var term = '/project.js';
        for (var n = scripts.length - 1; n > -1; n--) {
            var src = scripts[n].src.replace(/\?.*$/, ''); // Strip any query param (CB-6007).
            if (src.indexOf(term) === (src.length - term.length)) {
                path = src.substring(0, src.length - term.length) + '/';
                break;
            }
        }
        return path;
    };
    /** Validate Project Root Path  */
    navigator.project.rootPath = navigator.project._findProjectPath();
    if (navigator.project.rootPath == null) {
        navigator.project.rootPath = ""; // Note: Force relative project path
        console.warn("Could not find project.js script tag. Project loading may fail.");
    }
    /** Standard Project Dependencies */
    if (!window.PointerEvent) navigator.project._scriptLibraries.push("scripts/pep.js");
    if (!window.CANNON) navigator.project._scriptLibraries.push("scripts/cannon.js");
    if (!window.BABYLON) navigator.project._scriptLibraries.push("scripts/babylon.js");
    if (!(window.BABYLON && window.BABYLON.GUI)) navigator.project._scriptLibraries.push("scripts/babylon.gui.js");
    if (!(window.BABYLON && window.BABYLON.GLTFFileLoader)) navigator.project._scriptLibraries.push("scripts/babylon.gltf.js");
    if (!window.Navigation) navigator.project._scriptLibraries.push("scripts/babylon.navmesh.js");
    if (!window.INSPECTOR) navigator.project._scriptLibraries.push("scripts/babylon.inspector.js");
    if (!(window.BABYLON && window.BABYLON.SceneManager)) navigator.project._scriptLibraries.push("scripts/babylon.manager.js");
    /** Load Project Script References */
    navigator.project._scriptLibraries.push("scenes/NewUnityProject.babylon.js");
    navigator.project._loadProjectScripts(navigator.project._scriptLibraries, 0, "NewUnityProject.babylon");
} else {
    console.warn("The navigator project.js already created for " + navigator.project.tagName);
}
