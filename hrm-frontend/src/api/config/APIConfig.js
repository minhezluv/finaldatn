var APIConfig = {
  development: "http://hrm3-env.eba-qghius76.ap-southeast-1.elasticbeanstalk.com/",
  production:
    "http://hrm3-env.eba-qghius76.ap-southeast-1.elasticbeanstalk.com/",
};

export default APIConfig[process.env.NODE_ENV];
