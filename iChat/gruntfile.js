/// <binding BeforeBuild='sass' />
module.exports = function (grunt) {
    'use strict';

    var sass = require('node-sass');

    // Project configuration.
    grunt.initConfig({
        pkg: grunt.file.readJSON('package.json'),

        concat: {
            dist: {
                src: [
                    'Styles/*.scss'
                ],
                dest: 'Styles/site.scss'
            }
        },

        // Sass
        sass: {
            options: {
                implementation: sass,
                sourceMap: true, // Create source map
                //outputStyle: 'compressed' // Minify output
            },
            dist: {
                files: {
                    'wwwroot/css/site.css': 'Styles/site.scss'
                }
            }
        },

        clean: ['Styles/site.scss']
    });

    // Load the plugin
    grunt.loadNpmTasks('grunt-contrib-concat');
    grunt.loadNpmTasks('grunt-sass');
    grunt.loadNpmTasks('grunt-contrib-clean');

    // Default task(s).
    grunt.registerTask('default', ['concat', 'sass', 'clean']);
};