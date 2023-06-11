<template>
  <div
    v-if="show"
    class="create-dialog-overlay"
    @click="closeOnOverlayClick($event)"
  >
    <div class="create-dialog">
      <h2>Create Document</h2>
      <form @submit.prevent="submit">
        <label for="title">Title:</label>
        <input type="text" id="title" v-model="createdDocument.title" />

        <label for="content">Content:</label>
        <textarea id="content" v-model="createdDocument.content"></textarea>

        <label for="tags">Tags:</label>
        <input type="text" id="tags" v-model="createdDocument.tags" />

        <label for="type">Type:</label>
        <input type="text" id="type" v-model="createdDocument.type" />

        <label for="folder">Folder:</label>
        <select
          id="folder"
          v-model="createdDocument.folder"
          class="custom-select"
        >
          <option v-for="folder in folders" :key="folder" :value="folder">
            {{ folder.name }}
          </option>
        </select>

        <div class="create-dialog-actions">
          <button type="button" @click="cancel">Cancel</button>
          <button type="submit">Save</button>
        </div>
      </form>
    </div>
  </div>
</template>

<script>
export default {
  props: {
    folders: {
      type: Array,
    },
  },

  data() {
    return {
      show: false,
      createdDocument: {},
    };
  },

  methods: {
    open() {
      this.createdDocument = { ...this.document };
      this.show = true;
    },
    close() {
      this.show = false;
    },
    closeOnOverlayClick(event) {
      if (event.target === event.currentTarget) {
        this.close();
      }
    },
    cancel() {
      this.close();
    },
    submit() {
      this.$emit("document-created", { ...this.createdDocument });
      this.close();
    },
  },
};
</script>

<style scoped>
.custom-select {
  font-size: 14px;
  padding: 5px 10px;
  border: 1px solid #ccc;
  border-radius: 5px;
  margin-bottom: 15px;
  width: 96%; /* somehow this fixes the fields ¯\_(ツ)_/¯*/
  appearance: none;
  background: url("../assets/caret-down-solid.svg") no-repeat;
  background-position: right 10px center;
  background-size: 0.65em auto;
}

.create-dialog-overlay {
  position: fixed;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  background-color: rgba(0, 0, 0, 0.5);
  display: flex;
  justify-content: center;
  align-items: center;
  z-index: 1000;
}

.create-dialog {
  background-color: white;
  border-radius: 10px;
  padding: 30px;
  max-width: 550px;
  box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1), 0 1px 3px rgba(0, 0, 0, 0.08);
  display: flex;
  flex-direction: column;
}

h2 {
  font-weight: bold;
  font-size: 24px;
  margin-bottom: 20px;
}

label {
  font-weight: bold;
  font-size: 14px;
  margin-bottom: 5px;
}

input,
textarea {
  font-size: 14px;
  padding: 5px 10px;
  border: 1px solid #ccc;
  border-radius: 5px;
  margin-bottom: 15px;
  width: 96%; /* somehow this fixes the fields ¯\_(ツ)_/¯*/
}

textarea {
  height: 200px;
  overflow: auto;
  resize: none;
}
.create-dialog-actions {
  display: grid;
  grid-template-columns: 1fr 1fr;
  gap: 1em;
}

.custom-select {
  font-size: 14px;
  padding: 5px 30px 5px 10px;
  border: 1px solid #ccc;
  border-radius: 5px;
  margin-bottom: 15px;
  width: 100%;
  appearance: none;
  background: url("../assets/caret-down-solid.svg") no-repeat;
  background-position: right 10px center;
  background-size: 0.65em auto;
}

button {
  padding: 0.5em 1em;
  margin-right: 0.5em;
  margin-top: 20px;
  background-color: var(--icon-color);
  border: none;
  color: white;
  padding: 0.5em 1em;
  font-size: 1em;
  border-radius: 5px;
  cursor: pointer;
  transition: background-color 0.3s;
}

button:hover {
  background-color: var(--icon-hover-color);
}

button:first-child {
  background-color: var(--cancel-color);
}

button:first-child:hover {
  background-color: var(--cancel-hover-color);
}
</style>
