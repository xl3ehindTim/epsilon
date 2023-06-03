<template>
    <table class="competence-profile-legend">
        <tr
            v-for="level of props.domain.masteryLevels as MasteryLevel[]"
            :key="level.id"
            class="competence-profile-legend-row">
            <td :style="{ backgroundColor: level.color }" class="kpi-color" />
            <th class="kpi-text">Level {{ level.level }}</th>
        </tr>
    </table>
</template>

<script lang="ts" setup>
import { IHboIDomain, MasteryLevel } from "../logic/Api"

const props = defineProps<{
    domain: IHboIDomain
}>()
</script>

<style>
.competence-profile-legend {
    display: flex;
    flex-direction: column;
    width: fit-content;
    height: fit-content;
    padding: 2rem;
}

.kpi-text {
    font-weight: 400;
}

.kpi-color {
    width: 3rem;
}

.competence-profile-legend-row {
    display: grid;
    grid-template-columns: repeat(2, 5rem);
    padding: 0.5rem;
}
</style>
