const path = require('path');
var webpack = require("webpack");

module.exports = {
    entry: {
        vehicle: './Scripts/vehicle/VehicleIndexViewModel.js',
        journeyIndex: './Scripts/journey/JourneyIndexViewModel.js',
        journeyEdit: './Scripts/journey/JourneyEditViewModel.js',
    },
    resolve: {
        extensions: [".js"],
        alias: {
            vue: 'vue/dist/vue.js'
        }
    },
    module: {
        rules: [
            {
                test: /\.(js)$ /,
                exclude: /(node_modules|bower_components)/,
                use: {
                    loader: 'babel-loader',
                    options: {
                        presets: ['@babel/preset-env']
                    }
                }
            },
            {
                test: /\.css$/,
                use: [ 'style-loader', 'css-loader' ]
            }
        ]
    },
    plugins: [
        new webpack.optimize.CommonsChunkPlugin({
            name: 'vendor'
        })
    ],
    output: {
        filename: '[name].js',
        path: path.resolve(__dirname, 'wwwroot/js/')
    }
};