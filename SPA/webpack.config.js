const path = require('path');
const UglifyJSPlugin = require('uglifyjs-webpack-plugin');


module.exports =[
  {
      entry:{
      careers:['@babel/polyfill',"./src/Career.js"],
    },

    output:{
      path: path.resolve(__dirname, './dist/js'),
      filename: '[name].js'
    },

    optimization: {
    minimizer: [
      new UglifyJSPlugin({
        sourceMap: true,
        uglifyOptions: {
          compress: {
            inline: false
          }
        }
      })
    ],
    runtimeChunk: false,
    splitChunks: {
      cacheGroups: {
        default: false,
        commons: {
          test: /[\\/]node_modules[\\/]/,
          name: 'vendor_app',
          chunks: 'all',
          minChunks: 2
        }
      }
    }
  },
  module:{

    rules:[
      {
        test:/\.js$/, 
        exclude:[
        path.resolve(__dirname, "node_modules") 
        ],
          loader:"babel-loader",
          options:{
            presets:['@babel/env', '@babel/react'],
            plugins:['@babel/plugin-transform-runtime'],
          },
      },
      {
          test: /\.css$/,
          use: ['style-loader','css-loader']
          }
    ]
  },
  devtool: 'source-map'
  }
  
]

