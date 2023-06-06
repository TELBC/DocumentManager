<script setup>
import axios from "axios";
</script>

<template>
  <div class="document">
    <div
      class="documentsHeader"
      @click="loadContent(document.guid)"
      v-trim-whitespace
    >
      <div class="title" :title="document.title">{{ document.title }}</div>
      <div class="type" :title="document.type">{{ document.type }}</div>
      <div
        class="tags"
        v-if="document.tags.length > 0"
        @mouseover="hover = true"
        @mouseleave="hover = false"
      >
        <i class="fa-solid fa-tag tag-icon"></i>
        <div class="tag-list" v-if="hover">
          <ul>
            <li v-for="t in document.tags" :key="t">{{ t }}</li>
          </ul>
        </div>
      </div>

      <div class="version" :title="document.version">
        {{ document.version }}
      </div>
      <div class="actions">
        <span class="loadContent">
          <i class="fa-solid fa-file"></i>
        </span>
      </div>
    </div>
    <div class="documentContent">
      <div v-show="showContent">{{ getDocument(document.guid) }}</div>
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
      hover: false,
      showContent: false,
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
        this.showContent = !this.showContent;
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
  directives: {
    "trim-whitespace": trimWhitespace,
  },
  computed: {
    folderGuid() {
      return this.$route.params.id;
    },
  },
};
function trimEmptyTextNodes(el) {
  for (let node of el.childNodes) {
    if (node.nodeType === Node.TEXT_NODE && node.data.trim() === "") {
      node.remove();
    }
  }
}

var trimWhitespace = {
  inserted: trimEmptyTextNodes,
  componentUpdated: trimEmptyTextNodes,
};
</script>


<style scoped>
.document {
  background-color: #f5f5f5;
  border-radius: 10px;
  margin: 5px;
  padding: 10px;
  border: 1px solid #e0e0e0;
  width: 100%;
  max-width: 500px;
}

.documentsHeader {
  display: flex;
  flex-wrap: wrap;
  column-gap: 1.5em;
  margin-top: 0.1rem;
  padding-bottom: 5px;
}

.documentsHeader .version {
  flex-grow: 1;
}

.documentsHeader .actions {
  padding-right: 1rem;
  cursor: pointer;
  transition: all 0.2s;
}

.documentsHeader .actions:hover {
  color: var(--icon-hover-color);
}

.documentsHeader .title {
  flex: 0 0 10em;
  font-size: 1.1em;
  font-weight: bold;
}

.documentContent {
  padding-top: 5px;
  border-top: 1px solid #e0e0e0;
}

.documentsHeader:hover {
  background-color: #e9ecef;
}

.title,
.type,
.version {
  white-space: nowrap;
  overflow: hidden;
  text-overflow: ellipsis;
  max-width: 100%;
}

.tags {
  position: relative;
  display: flex;
  flex-wrap: wrap;
  gap: 0.5rem;
  cursor: pointer;
  transition: all 0.2s;
}

.tag-icon {
  margin-right: 0.2rem;
}

.tag-icon:hover {
  color: var(--icon-hover-color);
}

.tag-list {
  display: none;
  position: initial;
  min-width: fit-content;
  z-index: 1;
}

.tag-list ul {
  list-style-type: none;
  padding: 0;
  margin: 0;
}

.tag-list li {
  padding: 1px 0;
}

.tags:hover .tag-list {
  display: block;
}
</style>
