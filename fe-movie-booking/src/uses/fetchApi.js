import { ref } from "vue";
import api from "../services/api";

export default function useApi() {
  const data = ref(null);
  const error = ref(null);
  const isLoading = ref(false);

  const fetchData = async (method, url, requestData = null, params = {}) => {
    try {
      isLoading.value = true;
      const response = await api({ method, url, data: requestData, params });
      data.value = response.data;
      isLoading.value = false;
    } catch (err) {
      error.value = err;
      console.log("lỗi rồi", err);
      throw err;
    }
  };

  return { data, error, fetchData, isLoading };
}
