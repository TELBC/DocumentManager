<template>
  <div>
    <label for="guid">GUID:</label>
    <select id="guid" v-model="selectedGuid" @change="emitGuid">
      <option v-for="guid in guids" :key="guid" :value="guid">
        {{ guid }}
      </option>
    </select>
  </div>
</template>

<script>
export default {
  props: {
    guids: {
      type: Array,
      required: true,
    },
    initialGuid: {
      type: String,
      required: true,
    },
  },
  data() {
    return {
      selectedGuid: this.initialGuid,
    };
  },
  methods: {
    emitGuid() {
      this.$emit("guid-selected", this.selectedGuid);
    },
  },
};
</script>
