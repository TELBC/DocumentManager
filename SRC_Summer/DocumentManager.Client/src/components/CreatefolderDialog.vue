<template>
  <div
    v-if="show"
    class="create-dialog-overlay"
    @click="closeOnOverlayClick($event)"
  >
    <div class="create-dialog">
      <h2>Create Folder</h2>
      <form @submit.prevent="submit">
        <label for="name">Name:</label>
        <div>
          <input type="text" id="name" v-model="createdfolder.name" @input="clearError"/>
          <div v-if="validation.name" class="error">
            {{ validation.name }}
          </div>
        </div>

        <label for="documenttitles">Documents:</label>
        <div>
          <input
            type="text"
            id="documenttitles"
            v-model="createdfolder.DocumentTitles"
            @input="clearError"
          />
          <div v-if="validation.documentTitles" class="error">
            {{ validation.documentTitles }}
          </div>
        </div>

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
    validation: {
      type: Object,
      default: () => {},
    },
  },

  data() {
    return {
      show: false,
      createdfolder: {},
    };
  },

  methods: {
    open() {
      this.createdfolder = { ...this.folder };
      this.show = true;
    },
    close() {
      this.show = false;
    },
    closeOnOverlayClick(event) {
      if (event.target === event.currentTarget) {
        this.close();
        this.clearError();
      }
    },
    cancel() {
      this.close();
      this.clearError();
    },
    submit() {
      this.$emit("folder-created", this.createdfolder);
      this.clearError();
    },
    clearError() {
      this.$emit("clear-validation");
    },
  },
};
</script>

<style scoped>
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
  width: 550px;
  box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1), 0 1px 3px rgba(0, 0, 0, 0.08);
  display: flex;
  flex-direction: column;
}

.error {
  color: red;
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
