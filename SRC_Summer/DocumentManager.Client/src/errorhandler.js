import axios from "axios";

function errorResponseHandler(error) {
  if (error.response) {
    const errorMessage =
      error.response.data?.errorMessage || error.response.statusText;
    alert(`Error: ${error.response.status} - ${errorMessage}`);
  }
  return Promise.reject(error);
}

axios.interceptors.response.use((response) => response, errorResponseHandler);

export default errorResponseHandler;
