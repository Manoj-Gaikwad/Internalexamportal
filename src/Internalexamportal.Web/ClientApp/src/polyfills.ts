/**
 * This file includes polyfills needed by Angular and is loaded before the app.
 * You can add your own extra polyfills to this file.
 *
 * The current setup is for "evergreen" browsers (Safari >=10, Chrome >=55, Edge >=13).
 */

/***************************************************************************************************
 * BROWSER POLYFILLS
 */

/** IE9, IE10 and IE11 requires all of the following polyfills. **/
import 'core-js/features/symbol';
import 'core-js/features/object';
import 'core-js/features/function';
import 'core-js/features/parse-int';
import 'core-js/features/parse-float';
import 'core-js/features/number';
import 'core-js/features/math';
import 'core-js/features/string';
import 'core-js/features/date';
import 'core-js/features/array';
import 'core-js/features/regexp';
import 'core-js/features/map';
import 'core-js/features/weak-map';
import 'core-js/features/set';
import 'core-js/features/reflect';

/** IE10 and IE11 requires the following for NgClass support on SVG elements */
import 'classlist.js';  // npm install --save classlist.js

/**
 * Web Animations `@angular/platform-browser/animations`
 * Only required if AnimationBuilder is used and targeting IE/Edge or Safari.
 **/
import 'web-animations-js';  // npm install --save web-animations-js

/***************************************************************************************************
 * Zone JS is required by default for Angular itself.
 */
import 'zone.js';  // Included with Angular CLI

/***************************************************************************************************
 * APPLICATION IMPORTS
 */

// Add global to window
(window as any).global = window;

// Fix for ngx-charts to work on IE11
if (typeof SVGElement.prototype.contains === 'undefined') {
    SVGElement.prototype.contains = HTMLDivElement.prototype.contains;
}

// Polyfill for process.env to fix Angular 14 / Webpack 5 issue
(window as any).process = (window as any).process || {
    env: { NODE_ENV: 'production', DEBUG: undefined }
};
