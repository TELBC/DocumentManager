<script setup>
import axios from "axios";
import LoadingSpinner from "../components/LoadingSpinner.vue";
</script>

<template>
  <div class="folderView">
    <LoadingSpinner v-if="loading"></LoadingSpinner>
    <h1>Folder {{ folderGuid }}</h1>
    <div class="folders">
      <h3>Name: {{ folder.name }}</h3>
      <div class="documents" v-for="d in folder.documents" :key="d.guid">
        <div class="documentsHeader" @click="loadContent(d.guid)">
          <div class="title">{{ d.title }}</div>
          <div class="type">{{ d.type }}</div>
          <div class="tags" v-for="t in d.tags" :key="t">{{ t }}</div>
          <div class="version">{{ d.version }}</div>
          <div class="actions">
            <span class="loadContent">
              <i class="fa-solid fa-file"></i>
            </span>
          </div>
        </div>
        <div class="documentContent">
          {{ getDocument(d.guid) }}
        </div>
      </div>
    </div>
  </div>
</template>

<style scoped>
.documentsHeader {
  /* TODO: add css*/
  display: flex;
  flex-wrap: wrap;
  column-gap: 1.5em;
  border: 2px solid black;
  margin-top: 0.1rem;
}

.documentsHeader .version {
  flex-grow: 1;
}

.documentsHeader .actions {
  padding-right: 1rem;
}

.documentsHeader .title {
  flex: 0 0 10em;
}

.documentContent {
  border: 1px solid black;
}

.documentsHeader:hover {
  background-color: aliceblue;
}
</style>

<script>
export default {
  data() {
    return {
      folder: [],
      document: [],
      loading: false,
    };
  },
  async mounted() {
    this.folder = (await axios.get(`folders/${this.folderGuid}`)).data;
  },
  methods: {
    async loadContent(documentGuid) {
      if (this.loading) {
        return;
      }
      try {
        this.loading = true;
        this.document = [];
        const document = (
          await axios.get(`folders/${this.folderGuid}/${documentGuid}`)
        ).data;
        this.document.push(document);
      } catch (e) {
        alert("Server was unable to load document.");
      } finally {
        this.loading = false;
      }
    },
    getDocument(documentGuid) {
      return this.document.find((d) => d.guid === documentGuid);
    },
  },
  computed: {
    folderGuid() {
      return this.$route.params.id;
    },
  },
};
</script>