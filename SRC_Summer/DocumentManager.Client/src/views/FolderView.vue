<script setup>
import axios from "axios";
import LoadingSpinner from "../components/LoadingSpinner.vue";
import DocumentComponent from "../components/DocumentComponent.vue";
</script>

<template>
  <div class="folderView">
    <LoadingSpinner v-if="loading"></LoadingSpinner>
    <h1>Folder {{ folderGuid }}</h1>
    <div class="folders">
      <h3>Name: {{ folder.name }}</h3>
      <div class="documents" v-if="folder.documents">
        <DocumentComponent v-for="d in folder.documents" :key="d.guid" :document="d"></DocumentComponent>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  components: {
    DocumentComponent,
  },
  data() {
    return {
      folder: {},
      loading: false,
    };
  },
  async mounted() {
    this.loading = true;
    try {
      this.folder = (await axios.get(`folders/${this.folderGuid}`)).data;
    } catch (e) {
      alert("Server was unable to load folder.");
    } finally {
      this.loading = false;
    }
  },
  computed: {
    folderGuid() {
      return this.$route.params.id;
    },
  },
};
</script>
