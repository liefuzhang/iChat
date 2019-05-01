/// <binding BeforeBuild='sass' />
module.exports = function (grunt) {
    'use strict';

    var sass = require('node-sass');

    // Project configuration.
    grunt.initConfig({
        pkg: grunt.file.readJSON('package.json'),

        concat: {
            js: {
                src: [
                    'Scripts/*.js'
                ],
                dest: 'wwwroot/js/site.js'
            },
            css: {
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

        uglify: {
            options: {
                //compress: true,
                sourceMap: true
            },
            dist: {
                src: ['wwwroot/js/site.js'],
                dest: 'wwwroot/js/site.min.js'
            }
        },

        clean: ['Styles/site.scss']
    });

    // Load the plugin
    grunt.loadNpmTasks('grunt-contrib-concat');
    grunt.loadNpmTasks('grunt-sass');
    grunt.loadNpmTasks('grunt-contrib-clean');
    grunt.loadNpmTasks('grunt-contrib-uglify-es');

    // Default task(s).
    grunt.registerTask('default', ['concat', 'sass', 'uglify', 'clean']);
};