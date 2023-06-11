<script setup>
import axios from "../axios";
import LoadingSpinner from "../components/LoadingSpinner.vue";
import DocumentComponent from "../components/DocumentComponent.vue";
</script>

<template>
  <div class="folderView">
    <LoadingSpinner v-if="loading"></LoadingSpinner>
    <h1>{{ folder.name }}</h1>
    <div class="folders">
      <h4>guid: {{ folderGuid }}</h4>
      <div class="documents" v-if="folder.documents">
        <DocumentComponent
          v-for="d in folder.documents"
          :key="d.guid"
          :document="d"
          :documents="folder.documents"
          @document-deleted="loadFolder"
          @document-updated="loadFolder"
        ></DocumentComponent>
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
  methods: {
    async loadFolder() {
      this.loading = true;
      try {
        this.folder = (await axios.get(`folders/${this.folderGuid}`)).data;
      } catch (e) {
        alert("Server was unable to load folder."); //added global error handling
      } finally {
        this.loading = false;
      }
    },
  },
  async mounted() {
    this.loadFolder();
    this.loading = true;
  },
  computed: {
    folderGuid() {
      return this.$route.params.id;
    },
  },
};
</script>
