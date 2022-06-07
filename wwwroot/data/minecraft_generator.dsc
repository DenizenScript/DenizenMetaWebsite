# This is the Denizen script that generated 'minecraft.fds'

meta_extra_data_generator_task:
    type: task
    debug: false
    script:
    - definemap output:
        biomes: <server.biome_types.parse[name].alphabetical>
        blocks: <server.material_types.filter[is_block].parse[name].alphabetical>
        enchantments: <server.enchantment_keys.alphabetical>
        effects: <server.effect_types.alphabetical>
        potion_effects: <server.potion_effect_types.alphabetical>
        sounds: <server.sound_types.alphabetical>
        entities: <server.entity_types.alphabetical>
        potions: <server.potion_types.alphabetical>
        attributes: <server.nbt_attribute_types.alphabetical>
        particles: <server.particle_types.alphabetical>
        items: <server.material_types.filter[is_item].parse[name].alphabetical>
        gamerules: <server.gamerules.alphabetical>
        statistics: <server.statistic_types.alphabetical>
    - log type:none <[output].to_yaml> file:minecraft.fds
