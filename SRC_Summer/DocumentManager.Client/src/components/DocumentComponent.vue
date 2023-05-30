<script setup>
import axios from 'axios';
</script>

<template>
  <div class="document">
    <div class="documentsHeader" @click="loadContent(document.guid)">
      <div class="title">{{ document.title }}</div>
      <div class="type">{{ document.type }}</div>
      <div class="tags" v-for="t in document.tags" :key="t">{{ t }}</div>
      <div class="version">{{ document.version }}</div>
      <div class="actions">
        <span class="loadContent">
          <i class="fa-solid fa-file"></i>
        </span>
      </div>
    </div>
    <div class="documentContent">
      {{ getDocument(document.guid) }}
    </div>
  </div>
</template>

<script>
export default {
  props: {
    document: {
      type: Object,
      required: true,
    },
  },
  data() {
    return {
      loading: false,
      loadedDocument: null,
    };
  },
  methods: {
    async loadContent(documentGuid) {
      if (this.loading) {
        return;
      }
      try {
        this.loading = true;
        this.loadedDocument = (
          await axios.get(`folders/${this.folderGuid}/${documentGuid}`)
        ).data;
      } catch (e) {
        alert("Server was unable to load document.");
      } finally {
        this.loading = false;
      }
    },
    getDocument(documentGuid) {
      if (this.loadedDocument && this.loadedDocument.guid === documentGuid) {
        return this.loadedDocument.content;
      }
      return null;
    },
  },
  computed: {
    folderGuid() {
      return this.$route.params.id;
    },
  },
};
</script>

<style scoped>
.documentsHeader {
  /* TODO: add css */
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
