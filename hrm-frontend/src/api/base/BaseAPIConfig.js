import APIConfig from "../config/APIConfig";
import axios from "axios";

var BaseAPIConfig = axios.create({
  baseURL: APIConfig,
  headers: {
    "Content-type": "application/json",
    // "Access-Control-Allow-Origin": "*",
    // "Access-Control-Allow-Methods": "GET, POST, PATCH, PUT, DELETE, OPTIONS",
    // "Access-Control-Allow-Headers": "Origin, Content-Type, X-Auth-Token",
  },
});

export default BaseAPIConfig;
