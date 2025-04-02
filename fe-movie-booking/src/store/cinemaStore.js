// stores/cinemaStore.js
import { defineStore } from "pinia";
import useApi from "../uses/fetchApi";
import { ref } from "vue";

export const useCinemaStore = defineStore("cinemaStore", () => {
  const cinemas = ref([]);
  const selectedCinema = ref(null);
  const { data, error, fetchData } = useApi();

  const fetchCinemas = async () => {
    if (cinemas.value.length > 0) return;
    await fetchData("get", "/Cinema/all-cinema-name");
    if (data.value) {
      cinemas.value = data.value.$values;
      if (cinemas.value.length > 0) {
        selectedCinema.value = cinemas.value[0].id;
      }
    }
  };

  return { cinemas, error, selectedCinema, fetchCinemas };
});
